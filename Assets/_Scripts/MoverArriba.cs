using UnityEngine;

public class MoverArriba : MonoBehaviour
{
    public float velocidadMin = 1f;
    public float velocidadMax = 4f;
    public float alturaParaDestruir = 7f;

    private float velocidadBase;

    void Start()
    {
        velocidadBase = Random.Range(velocidadMin, velocidadMax);
    }

    void Update()
    {
        // Leemos la velocidad turbo del GameManager
        float multiplicadorTurbo = 1f;

        if (GameManager.instance != null)
        {
            multiplicadorTurbo = GameManager.instance.velocidadMundo;
        }

        // Movimiento
        transform.Translate(Vector2.up * velocidadBase * multiplicadorTurbo * Time.deltaTime);

        // Limpieza (Arriba y Abajo)
        if (transform.position.y > 8f || transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }
}