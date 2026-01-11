using UnityEngine;

public class EasterEggDuck : MonoBehaviour
{
    [Header("Configuración")]
    public Sprite patoCoolSprite; // <-- ARRASTRA AQUÍ LA IMAGEN DEL PATO CON LENTES

    private Sprite patoNormalSprite; // Para guardar el original
    private SpriteRenderer rend;
    private AudioSource audioSource;
    private bool encontrado = false;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Guardar el sprite original por si acaso
        patoNormalSprite = rend.sprite;

        // REVISAR MEMORIA: ¿Ya lo encontraron antes?
        if (PlayerPrefs.GetInt("DuckFound", 0) == 1)
        {
            EquiparLentes(); // Si ya lo encontraron, inicia "cool" directamente
        }
    }

    void OnMouseDown()
    {
        // 1. Sonido gracioso
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(audioSource.clip);
        }

        // 2. Efecto Visual (Pequeño salto de "susto")
        transform.localScale = Vector3.one * 1.2f;
        Invoke("ResetSize", 0.1f);

        // 3. Guardar el secreto (Solo la primera vez)
        if (!encontrado)
        {
            encontrado = true;
            PlayerPrefs.SetInt("DuckFound", 1);
            PlayerPrefs.Save();
            Debug.Log("¡CUACK! Easter Egg Encontrado.");

            // ¡Transformación!
            EquiparLentes();
        }
    }

    void ResetSize()
    {
        transform.localScale = Vector3.one;
    }

    void EquiparLentes()
    {
        // Si tenemos el sprite cool asignado, lo cambiamos
        if (patoCoolSprite != null)
        {
            rend.sprite = patoCoolSprite;
        }
    }

    // OPCIONAL: Si necesitas borrar la memoria para probar el cambio
    public void ResetearPato()
    {
        PlayerPrefs.DeleteKey("DuckFound");
        rend.sprite = patoNormalSprite;
        encontrado = false;
        Debug.Log("Memoria del pato reseteada.");
    }
}