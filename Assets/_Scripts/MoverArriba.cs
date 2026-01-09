using UnityEngine;

public class MoverArriba : MonoBehaviour
{
    [Header("Velocidad Variada")]
    public float velocidadMin = 1f;
    public float velocidadMax = 4f;

    [Header("Limpieza")]
    public float alturaParaDestruir = 6f; // Justo por encima de donde ve la cámara

    private float velocidadReal;

    void Start()
    {
        // Al nacer, este objeto elige su propia personalidad (velocidad)
        velocidadReal = Random.Range(velocidadMin, velocidadMax);
    }

    void Update()
    {
        // Leemos la velocidad de la "antena" (GameManager.instance.velocidadMundo)
        float velocidadTotal = velocidadReal * GameManager.instance.velocidadMundo;

        // Movemos
        transform.Translate(Vector2.up * velocidadTotal * Time.deltaTime);

        // ... (El resto de tu código de limpieza sigue igual) ...
        if (transform.position.y > 6f || transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }
}