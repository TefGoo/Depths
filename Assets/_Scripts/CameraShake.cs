using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Singleton para llamarlo fácil desde cualquier lado
    public static CameraShake instance;

    private Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.position; // Guardamos donde vive la cámara (0,0,-10)
    }

    public void Shake(float duracion, float magnitud)
    {
        StartCoroutine(DoShake(duracion, magnitud));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Generar un punto aleatorio muy cerca del centro
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Mover la cámara ahí
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            // Esperar al siguiente frame
            yield return null;
        }

        // Al terminar, regresar a casa
        transform.position = originalPos;
    }
}