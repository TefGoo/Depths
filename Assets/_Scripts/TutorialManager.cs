using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Referencia UI")]
    public GameObject tutorialPanel; // Arrastra aquí tu Panel de Tutorial

    private bool estaAbierto = true; // Asumimos que empieza abierto

    void Start()
    {
        // Al iniciar, nos aseguramos de abrirlo y pausar
        AbrirTutorial();
    }

    void Update()
    {
        // Detectar tecla ESC
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
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el tiempo
    }

    public void CerrarTutorial()
    {
        estaAbierto = false;
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // Reanuda el tiempo
    }
}