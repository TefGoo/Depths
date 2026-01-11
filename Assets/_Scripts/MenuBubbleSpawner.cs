using UnityEngine;

public class MenuBubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // Tu prefab de burbuja (bolita blanca)
    public float spawnInterval = 0.5f; // Salen rápido
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBubble();
            timer = 0f;
        }
    }

    void SpawnBubble()
    {
        if (bubblePrefab == null) return;

        // Posición aleatoria en el ancho de la pantalla (ajusta el -8 y 8)
        float randomX = Random.Range(-8f, 8f);

        // Posición fija abajo de la pantalla (fuera de cámara)
        float spawnY = -6f;

        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);

        Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
    }
}