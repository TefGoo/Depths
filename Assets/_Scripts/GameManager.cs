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
    public int repairCost = 15;
    public int oxygenCost = 20;
    public int armorCost = 25;

    [Header("Estadísticas (Mejoras)")]
    public float cablePower = 10f;

    [Header("Referencias UI")]
    public TextMeshProUGUI depthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;

    [Header("UI Oxígeno (1-BIT)")]
    public Slider oxygenBar; // Solo necesitamos esto, haremos parpadear todo el objeto
    // (Borré la variable oxygenFillImage porque ya no la usamos)

    [Header("Panel Victoria")]
    public GameObject winPanel;

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

        UpdateUI();

        if (winPanel != null) winPanel.SetActive(false);
    }

    void Update()
    {
        // 1. GESTIÓN DE OXÍGENO
        if (currentDepth > 0 && !isReturning)
        {
            currentOxygen -= Time.deltaTime;
            if (currentOxygen <= 0) Die("¡ASFIXIA!");
        }

        // Parpadeo de TODO el slider
        HandleOxygenVisuals();

        // 2. GESTIÓN DE MOVIMIENTO Y RETORNO
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

    // --- LÓGICA VISUAL DEL OXÍGENO (TODO PARPADEA) ---
    void HandleOxygenVisuals()
    {
        if (oxygenBar == null) return;

        // Actualizar valor
        float porcentaje = currentOxygen / maxOxygen;
        oxygenBar.value = porcentaje;

        // Lógica de Parpadeo GLOBAL
        if (porcentaje > 0.5f)
        {
            // Seguro: Siempre visible
            if (!oxygenBar.gameObject.activeSelf)
                oxygenBar.gameObject.SetActive(true);
        }
        else
        {
            // Peligro: Parpadeo
            float blinkSpeed = 5f; // Velocidad media

            if (porcentaje <= 0.25f)
            {
                blinkSpeed = 15f; // Velocidad ALTA (Pánico)
            }

            // Onda Senoidal (-1 a 1)
            float onda = Mathf.Sin(Time.time * blinkSpeed);

            // Si la onda es positiva prendemos, si es negativa apagamos
            bool debeVerse = (onda > 0);

            // Solo hacemos el cambio si es diferente al estado actual (optimización)
            if (oxygenBar.gameObject.activeSelf != debeVerse)
            {
                oxygenBar.gameObject.SetActive(debeVerse);
            }
        }
    }

    // --- ACCIONES DEL JUGADOR ---

    public void Descend()
    {
        if (isReturning) return;

        currentDepth += cablePower;
        velocidadMundo = 10f;

        if (currentDepth >= profundidadMeta) WinGame();

        UpdateUI();
    }

    public void ActivatePanic()
    {
        if (currentDepth > 0)
        {
            isReturning = true;
            Debug.Log("¡EMERGENCIA! INICIANDO ASCENSO...");
        }
    }

    // --- LÓGICA INTERNA ---

    void LlegadaSuperficie()
    {
        isReturning = false;
        velocidadMundo = 1f;
        currentOxygen = maxOxygen;

        // IMPORTANTE: Asegurar que el slider esté visible al llegar
        if (oxygenBar != null) oxygenBar.gameObject.SetActive(true);

        UpdateUI();

        MoverArriba[] basuraFlotante = FindObjectsOfType<MoverArriba>();
        foreach (MoverArriba objeto in basuraFlotante)
        {
            Destroy(objeto.gameObject);
        }
    }

    // --- TIENDA ---

    public void BuyCableUpgrade()
    {
        if (totalMoney >= cableCost)
        {
            totalMoney -= cableCost;
            cablePower += 5f;
            cableCost *= 2;
            UpdateUI();
        }
    }

    public void BuyRepairHull()
    {
        if (totalMoney >= repairCost && currentHealth < maxHealth)
        {
            totalMoney -= repairCost;
            currentHealth += 30;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
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
            UpdateUI();
        }
    }

    // --- EVENTOS ---

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isReturning) return;

        currentHealth -= damage;
        UpdateUI();

        if (CameraShake.instance != null) CameraShake.instance.Shake(0.2f, 0.3f);

        if (currentHealth <= 0) Die("¡CASCO ROTO!");
    }

    // --- UI ---

    void UpdateUI()
    {
        // --- HUD PRINCIPAL (MAIN HUD) ---
        if (depthText != null) depthText.text = currentDepth.ToString("F0") + " m";
        if (moneyText != null) moneyText.text = totalMoney + " DATA"; // Antes DATOS
        if (healthText != null) healthText.text = "HEALTH: " + currentHealth + "%"; // Antes CASCO

        // --- TIENDA (STORE BUTTONS) ---

        // 1. CABLE (Pwr = Power)
        if (btnCableText != null)
            btnCableText.text = "CABLE\n(Pwr: " + cablePower.ToString("F0") + "m)\n$" + cableCost;

        // 2. REPARAR (REPAIR)
        if (btnRepairText != null)
        {
            if (currentHealth >= maxHealth)
                btnRepairText.text = "REPAIR\n(Full)\n--";
            else
                btnRepairText.text = "REPAIR\n(HP: " + currentHealth + "%)\n$" + repairCost;
        }

        // 3. OXÍGENO (OXYGEN)
        if (btnOxygenText != null)
            btnOxygenText.text = "OXYGEN\n(" + maxOxygen.ToString("F0") + "s Max)\n$" + oxygenCost;

        // 4. BLINDAJE (ARMOR)
        if (btnArmorText != null)
        {
            if (collisionDamage > 5)
            {
                int currentLevel = (30 - collisionDamage) / 5;
                btnArmorText.text = "ARMOR\n(Lvl " + currentLevel + "/5)\n$" + armorCost;
            }
            else
            {
                btnArmorText.text = "ARMOR\n(MAX)\n--";
            }
        }
    }

    void Die(string reason)
    {
        Debug.Log("GAME OVER: " + reason);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void WinGame()
    {
        velocidadMundo = 0f;
        if (winPanel != null) winPanel.SetActive(true);
    }
}