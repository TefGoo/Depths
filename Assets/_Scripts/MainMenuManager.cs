using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Referencias")]
    public GameObject panelCreditos; // Arrastra tu panel de créditos aquí

    void Start()
    {
        // Asegurarnos que al volver al menú el tiempo corra normal
        Time.timeScale = 1f;

        if (panelCreditos != null) panelCreditos.SetActive(false);
    }
    void Update()
    {
        // ESCUCHAR TECLA ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si el panel de créditos existe y está abierto...
            if (panelCreditos != null && panelCreditos.activeSelf)
            {
                // ...lo cerramos.
                panelCreditos.SetActive(false);
            }
        }
    }
    public void PlayGame()
    {
        // CAMBIA "GameScene" POR EL NOMBRE EXACTO DE TU ESCENA DE JUEGO
        SceneManager.LoadScene("SampleScene");
    }

    public void ToggleCredits()
    {
        if (panelCreditos != null)
        {
            // Si está prendido lo apaga, si está apagado lo prende
            panelCreditos.SetActive(!panelCreditos.activeSelf);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}