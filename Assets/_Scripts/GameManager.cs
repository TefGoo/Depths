using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- SINGLETON (La Antena) ---
    public static GameManager instance;

    [Header("Configuración General")]
    public float velocidadMundo = 1f; // 1 = Normal, 10 = Turbo

    [Header("Estadísticas de Juego")]
    public float currentDepth = 0f;
    public int totalMoney = 0;

    [Header("Salud / Casco")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Tienda / Mejoras")]
    public int cableCost = 10;
    public float cablePower = 10f; // Cuánto bajas por click

    [Header("Condición de Victoria")]
    public float targetDepth = 10000f; // La meta
    public GameObject winPanel; // Arrastra tu panel aquí
    private bool gameEnded = false;

    [Header("Referencias UI (Arrastra los textos aquí)")]
    public TextMeshProUGUI depthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI upgradeText;

    void Awake()
    {
        // El Singleton se inicializa siempre en Awake
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si recargas escena
        }
    }

    void Start()
    {
        // 1. Inicializar Salud
        currentHealth = maxHealth;
        UpdateHealthUI();

        // 2. Inicializar Textos Base
        if (depthText != null) depthText.text = "0 m";
        if (moneyText != null) moneyText.text = totalMoney + " DATOS";

        // 3. Inicializar Texto Tienda
        if (upgradeText != null)
            upgradeText.text = "MEJORAR CABLE\n($" + cableCost + ")";
    }

    void Update()
    {
        // Lógica de frenado del Turbo (Vuelve a 1 poco a poco)
        if (velocidadMundo > 1f)
        {
            velocidadMundo -= Time.deltaTime * 5f;
        }
        else
        {
            velocidadMundo = 1f;
        }
    }

    // --- ACCIONES DEL JUGADOR ---

    public void Descend()
    {
        if (gameEnded) return; // Si ya ganó o perdió, el botón no hace nada

        // Bajar
        currentDepth += cablePower;

        // Actualizar Texto
        if (depthText != null)
            depthText.text = currentDepth.ToString("F0") + " m";

        // Checar Victoria
        if (currentDepth >= targetDepth)
        {
            WinGame();
        }

        // Efecto turbo
        velocidadMundo = 10f;
    }

    public void BuyCableUpgrade()
    {
        if (totalMoney >= cableCost)
        {
            // Cobrar y Mejorar
            totalMoney -= cableCost;
            cablePower += 5f;      // Ahora bajas más rápido
            cableCost *= 2;        // Inflación de precio

            // Actualizar UI
            if (moneyText != null) moneyText.text = totalMoney + " DATOS";
            if (upgradeText != null) upgradeText.text = "MEJORAR CABLE\n($" + cableCost + ")";
        }
        else
        {
            Debug.Log("¡No tienes suficientes Datos!");
        }
    }

    // --- SISTEMA DE EVENTOS (Daño y Dinero) ---

    public void AddMoney(int cantidad)
    {
        totalMoney += cantidad;
        if (moneyText != null) moneyText.text = totalMoney + " DATOS";
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("¡Golpe! Salud restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }

        UpdateHealthUI();
    }

    // --- FUNCIONES INTERNAS ---

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "CASCO: " + currentHealth + "%";
    }

    public void GameOver()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void WinGame()
    {
        gameEnded = true;
        Debug.Log("¡HAS LLEGADO AL FONDO!");

        // Mostrar el panel
        if (winPanel != null) winPanel.SetActive(true);

        // Detener el caos (Opcional: poner velocidad a 0)
        velocidadMundo = 0f;
    }
}