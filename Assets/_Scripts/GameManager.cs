using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // --- SINGLETON ---
    public static GameManager instance;

    [Header("Configuración Global")]
    public float velocidadMundo = 1f;
    public float gravedadVisual = 5f;
    public float velocidadAscenso = 100f;
    public float profundidadMeta = 10000f;

    [Header("Estados del Juego")]
    public bool isReturning = false;
    public float currentDepth = 0f;
    private bool isGameOver = false; // Nueva bandera para evitar errores

    [Header("Supervivencia")]
    public float maxOxygen = 30f;
    public float currentOxygen;
    public int maxHealth = 100;
    private int currentHealth;
    public int collisionDamage = 25;

    [Header("Economía")]
    public int totalMoney = 0;

    [Header("Costos de Tienda")]
    public int cableCost = 10;
    public int repairCost = 20;
    public int oxygenCost = 20;
    public int armorCost = 25;

    [Header("Estadísticas (Mejoras)")]
    public float cablePower = 10f;

    [Header("Audio SFX")]
    public AudioSource audioSource;
    public AudioClip sfxCollect;
    public AudioClip sfxHit;
    public AudioClip sfxDive;
    public AudioClip sfxPanic;
    public AudioClip sfxBuy;
    public AudioClip sfxWarning;

    private float nextWarningTime = 0f;

    [Header("Referencias UI")]
    public TextMeshProUGUI depthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    public Slider oxygenBar;

    [Header("Referencias UI (Paneles)")]
    public GameObject winPanel;
    public GameObject gameOverPanel;        // NUEVO: Panel de Derrota
    public TextMeshProUGUI finalDepthText;  // NUEVO: Texto para mostrar la altura final

    [Header("Referencias UI (Botones Tienda)")]
    public TextMeshProUGUI btnCableText;
    public TextMeshProUGUI btnRepairText;
    public TextMeshProUGUI btnOxygenText;
    public TextMeshProUGUI btnArmorText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;
        isGameOver = false;

        // Aseguramos que el tiempo corra (por si reiniciamos después de morir)
        Time.timeScale = 1f;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        UpdateUI();

        // Esconder paneles al inicio
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return; // Si morimos, no actualizar nada más

        // 1. GESTIÓN DE OXÍGENO
        if (currentDepth > 0 && !isReturning)
        {
            currentOxygen -= Time.deltaTime;
            if (currentOxygen <= 0) Die("OXYGEN DEPLETED!");
        }

        HandleOxygenVisualsAndSound();

        // 2. GESTIÓN DE MOVIMIENTO
        if (isReturning)
        {
            currentDepth -= velocidadAscenso * Time.deltaTime;
            velocidadMundo = -20f;

            if (currentDepth <= 0)
            {
                currentDepth = 0;
                LlegadaSuperficie();
            }
            UpdateUI();
        }
        else
        {
            if (velocidadMundo > 1f)
                velocidadMundo -= Time.deltaTime * gravedadVisual;
            else if (velocidadMundo < 1f)
                velocidadMundo = 1f;
        }
    }

    // --- LÓGICA DE DERROTA Y BOTONES (NUEVO) ---

    void Die(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER: " + reason);

        // 1. Pausar el juego (Congelar todo)
        Time.timeScale = 0f;

        // 2. Mostrar Panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // 3. Mostrar la profundidad alcanzada
            if (finalDepthText != null)
            {
                finalDepthText.text = "YOU DIED AT\n" + currentDepth.ToString("F0") + " METERS";
            }
        }
    }

    public void RestartGame() // Botón REINTENTAR
    {
        Time.timeScale = 1f; // Descongelar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() // Botón MENÚ
    {
        Time.timeScale = 1f;
        // IMPORTANTE: Asegúrate de que tu escena de menú se llame "MainMenu"
        // o cambia este nombre por el de tu escena real.
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() // Botón SALIR
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // --- RESTO DEL CÓDIGO (Visuales, Tienda, etc.) ---

    void HandleOxygenVisualsAndSound()
    {
        if (oxygenBar == null) return;

        float porcentaje = currentOxygen / maxOxygen;
        oxygenBar.value = porcentaje;

        if (porcentaje > 0.5f)
        {
            if (!oxygenBar.gameObject.activeSelf) oxygenBar.gameObject.SetActive(true);
        }
        else
        {
            float blinkSpeed = 10f;
            float beepInterval = 0.5f;

            if (porcentaje <= 0.25f)
            {
                blinkSpeed = 25f;
                beepInterval = 0.2f;
            }

            float onda = Mathf.Sin(Time.time * blinkSpeed);
            bool debeVerse = (onda > 0);
            if (oxygenBar.gameObject.activeSelf != debeVerse)
                oxygenBar.gameObject.SetActive(debeVerse);

            if (Time.time > nextWarningTime && currentDepth > 0 && !isReturning)
            {
                PlaySFX(sfxWarning);
                nextWarningTime = Time.time + beepInterval;
            }
        }
    }

    public void Descend()
    {
        if (isReturning || isGameOver) return;

        currentDepth += cablePower;
        velocidadMundo = 10f;

        PlaySFX(sfxDive);

        if (currentDepth >= profundidadMeta) WinGame();

        UpdateUI();
    }

    public void ActivatePanic()
    {
        if (currentDepth > 0 && !isGameOver)
        {
            isReturning = true;
            PlaySFX(sfxPanic);
            Debug.Log("PANIC! RETURNING...");
        }
    }

    void LlegadaSuperficie()
    {
        isReturning = false;
        velocidadMundo = 1f;
        currentOxygen = maxOxygen;

        if (oxygenBar != null) oxygenBar.gameObject.SetActive(true);

        UpdateUI();

        MoverArriba[] basuraFlotante = FindObjectsOfType<MoverArriba>();
        foreach (MoverArriba objeto in basuraFlotante)
        {
            Destroy(objeto.gameObject);
        }
    }

    public void BuyCableUpgrade()
    {
        if (totalMoney >= cableCost)
        {
            totalMoney -= cableCost;
            cablePower += 5f;
            cableCost *= 2;
            PlaySFX(sfxBuy);
            UpdateUI();
        }
    }

    public void BuyRepairHull()
    {
        if (totalMoney >= repairCost && currentHealth < maxHealth)
        {
            totalMoney -= repairCost;
            currentHealth += 20; // +20 HP Fijo
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            PlaySFX(sfxBuy);
            UpdateUI();
        }
    }

    public void BuyOxygenUpgrade()
    {
        if (totalMoney >= oxygenCost)
        {
            totalMoney -= oxygenCost;
            maxOxygen += 10f;
            currentOxygen += (maxOxygen * 0.20f);
            if (currentOxygen > maxOxygen) currentOxygen = maxOxygen;
            oxygenCost += 10;
            PlaySFX(sfxBuy);
            UpdateUI();
        }
    }

    public void BuyArmorUpgrade()
    {
        if (totalMoney >= armorCost && collisionDamage > 5)
        {
            totalMoney -= armorCost;
            collisionDamage -= 5;
            armorCost += 20;
            PlaySFX(sfxBuy);
            UpdateUI();
        }
    }

    public void AddMoney(int amount)
    {
        if (isGameOver) return;
        totalMoney += amount;
        PlaySFX(sfxCollect);
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isReturning || isGameOver) return;

        currentHealth -= damage;
        PlaySFX(sfxHit);
        UpdateUI();

        if (CameraShake.instance != null) CameraShake.instance.Shake(0.2f, 0.3f);

        if (currentHealth <= 0) Die("HULL BREACHED!");
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
        }
    }

    void UpdateUI()
    {
        if (depthText != null) depthText.text = currentDepth.ToString("F0") + " m";
        if (moneyText != null) moneyText.text = totalMoney + " DATA";
        if (healthText != null) healthText.text = "HEALTH: " + currentHealth + "%";

        if (btnCableText != null)
            btnCableText.text = "UPGRADE\n+5 M\n[ $" + cableCost + " ]";

        if (btnRepairText != null)
        {
            if (currentHealth >= maxHealth) btnRepairText.text = "FULL\nHP";
            else btnRepairText.text = "REPAIR\n+20 HP\n[ $" + repairCost + " ]";
        }

        if (btnOxygenText != null)
            btnOxygenText.text = "TANK\n+10 S\n[ $" + oxygenCost + " ]";

        if (btnArmorText != null)
        {
            if (collisionDamage > 5)
                btnArmorText.text = "ARMOR\n-5 DMG\n[ $" + armorCost + " ]";
            else btnArmorText.text = "ARMOR\nMAX\n--";
        }
    }

    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        velocidadMundo = 0f;
        Time.timeScale = 0f; // Pausa al ganar también
        if (winPanel != null) winPanel.SetActive(true);
    }
}