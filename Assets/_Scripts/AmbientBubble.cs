using UnityEngine;

public class AmbientBubble : MonoBehaviour
{
    public float speed = 1.5f;     // Velocidad hacia arriba
    public float wobbleSpeed = 4f; // Qué tan rápido se mueve de lado a lado
    public float wobbleSize = 0.3f;// Qué tan ancho es el movimiento

    private float startX;
    private float timeOffset;

    void Start()
    {
        startX = transform.position.x;
        timeOffset = Random.Range(0f, 10f); // Para que cada una sea única

        // Variar un poco el tamaño para que se vea natural
        float scale = Random.Range(0.5f, 1.2f);
        transform.localScale = Vector3.one * scale;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // 1. Subir (Eje Y)
        pos.y += speed * Time.deltaTime;

        // 2. Bamboleo lateral (Eje X) usando Seno
        pos.x = startX + Mathf.Sin((Time.time * wobbleSpeed) + timeOffset) * wobbleSize;

        transform.position = pos;

        // 3. Destruir si sale por arriba (ajusta el 7f según tu cámara)
        if (pos.y > 7f)
        {
            Destroy(gameObject);
        }
    }
}