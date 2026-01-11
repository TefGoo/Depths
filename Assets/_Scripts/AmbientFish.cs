using UnityEngine;

public class AmbientFish : MonoBehaviour
{
    [Header("Velocidad")]
    public float speed = 2f;
    public float direction = 1f; // 1 = Derecha, -1 = Izquierda

    [Header("Efecto de Nado (ZigZag)")]
    public float frecuencia = 3f;  // Qué tan rápido aletea (sube y baja)
    public float amplitud = 0.5f;  // Qué tanto se aleja del centro (altura de la onda)

    private float startY;          // Guardamos la altura inicial
    private float desfase;         // Para que no todos los peces se muevan igual

    void Start()
    {
        startY = transform.position.y;

        // Generamos un número aleatorio para que cada pez empiece la onda en un punto distinto.
        // Si no hacemos esto, todos los peces subirían y bajarían al mismo tiempo (como robots).
        desfase = Random.Range(0f, 10f);
    }

    void Update()
    {
        // Calculamos la nueva posición
        Vector3 pos = transform.position;

        // 1. Movimiento Horizontal (Constante)
        pos.x += speed * direction * Time.deltaTime;

        // 2. Movimiento Vertical (Onda Matemática)
        // Fórmula: Y_Inicial + Seno(Tiempo * Velocidad + Aleatoriedad) * Distancia
        pos.y = startY + Mathf.Sin((Time.time * frecuencia) + desfase) * amplitud;

        // Aplicar cambios
        transform.position = pos;

        // 3. Limpieza (Destruir si sale de pantalla)
        if (Mathf.Abs(transform.position.x) > 15f)
        {
            Destroy(gameObject);
        }
    }
}