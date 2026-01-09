using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // --- ESTO ES NUEVO (LA ANTENA) ---
    public static GameManager instance;
    public float velocidadMundo = 1f; // 1 = Normal, 5 = Rápido
    // ---------------------------------

    [Header("UI References")]
    public TextMeshProUGUI depthText;

    [Header("Game Settings")]
    public float currentDepth = 0f;
    public float depthPerClick = 10f;

    void Awake()
    {
        // Esto configura la antena para que funcione
        instance = this;
    }

    void Update()
    {
        // La velocidad siempre intenta volver a la normalidad (1)
        // El "5f" es qué tan rápido se frena el turbo.
        if (velocidadMundo > 1f)
        {
            velocidadMundo -= Time.deltaTime * 5f;
        }
        else
        {
            velocidadMundo = 1f;
        }
    }

    public void Descend()
    {
        currentDepth += depthPerClick;
        depthText.text = currentDepth.ToString("F0") + " m";

        // --- AQUÍ ACTIVAMOS EL TURBO ---
        velocidadMundo = 10f; // ¡Bum! Acelerón instantáneo
    }
}