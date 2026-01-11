using UnityEngine;

public class MenuAmbientSpawner : MonoBehaviour
{
    [Header("Colección de Peces")]
    public GameObject[] fishPrefabs; // <--- AHORA ES UN ARRAY (Lista)

    [Header("Configuración")]
    public float spawnInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFish();
            timer = 0f;
        }
    }

    void SpawnFish()
    {
        // 0. Seguridad: Si no hay peces en la lista, no hacemos nada
        if (fishPrefabs.Length == 0) return;

        // 1. Elegir un pez al azar de la lista
        int randomIndex = Random.Range(0, fishPrefabs.Length);
        GameObject pezElegido = fishPrefabs[randomIndex];

        // 2. Decidir lado (Izquierda o Derecha)
        int side = Random.Range(0, 2);
        float spawnX = (side == 0) ? -10f : 10f;
        float direction = (side == 0) ? 1f : -1f;

        float spawnY = Random.Range(-4f, 4f);

        Vector3 pos = new Vector3(spawnX, spawnY, 0);

        // 3. Crear el pez elegido
        GameObject fish = Instantiate(pezElegido, pos, Quaternion.identity);

        // 4. Configurar movimiento
        AmbientFish script = fish.GetComponent<AmbientFish>();
        if (script != null)
        {
            script.direction = direction;

            // Variar un poco la velocidad para que no naden todos igual
            script.speed = Random.Range(1.5f, 3.5f);
        }

        // 5. Voltear sprite si va a la izquierda
        if (direction == -1)
        {
            Vector3 scale = fish.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            fish.transform.localScale = scale;
        }
    }
}