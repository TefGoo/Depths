using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Referencia UI")]
    public GameObject tutorialPanel;

    private bool estaAbierto = false; // Empezamos asumiendo que está cerrado

    void Start()
    {
        // 1. Preguntamos a la memoria: "¿Ya vio el tutorial?" (0 = No, 1 = Sí)
        if (PlayerPrefs.GetInt("TutorialVisto", 0) == 0)
        {
            // --- PRIMERA VEZ QUE JUEGA ---
            AbrirTutorial();

            // Guardamos en memoria que YA lo vio para la próxima
            PlayerPrefs.SetInt("TutorialVisto", 1);
            PlayerPrefs.Save();
        }
        else
        {
            // --- YA ES UN VETERANO ---
            // Nos aseguramos de que empiece cerrado y el juego corriendo
            CerrarTutorial();
        }
    }

    void Update()
    {
        // Detectar tecla ESC para abrirlo si tienen dudas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (estaAbierto)
            {
                CerrarTutorial();
            }
            else
            {
                AbrirTutorial();
            }
        }
    }

    public void AbrirTutorial()
    {
        estaAbierto = true;
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa
    }

    public void CerrarTutorial()
    {
        estaAbierto = false;
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // Play
    }

    // OPCIONAL: Llama a esta función si quieres borrar la memoria (útil para pruebas)
    public void ResetearTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialVisto");
        Debug.Log("Memoria de tutorial borrada.");
    }
}