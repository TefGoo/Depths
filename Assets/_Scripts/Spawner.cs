using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject burbujaPrefab;
    public GameObject datoPrefab;
    public GameObject enemigoPrefab;

    [Header("Configuración Base")]
    public float tiempoEntreSpawns = 0.5f;
    [Range(0, 100)] public float probabilidadDato = 10f;     // Chance de Dinero
    [Range(0, 100)] public float probabilidadEnemigo = 15f;  // Chance Inicial de Enemigos

    [Header("Variación (Solo Burbujas)")]
    public float tamanoMin = 0.5f;
    public float tamanoMax = 1.5f;

    private float tiempoSiguiente = 0f;

    void Update()
    {
        if (Time.time > tiempoSiguiente)
        {
            GenerarObjeto();
            tiempoSiguiente = Time.time + tiempoEntreSpawns;
        }
    }

    void GenerarObjeto()
    {
        // --- 1. CALCULAR DIFICULTAD (EVOLUCIÓN) ---
        // Obtenemos la profundidad actual desde el GameManager
        float profundidadActual = 0f;
        if (GameManager.instance != null)
            profundidadActual = GameManager.instance.currentDepth;

        // Empezamos con la probabilidad base (ej. 15%)
        float chanceEnemigoReal = probabilidadEnemigo;

        // Aumentamos la dificultad según bajamos
        if (profundidadActual > 2000f) chanceEnemigoReal = 30f; // Zona Peligrosa
        if (profundidadActual > 5000f) chanceEnemigoReal = 50f; // Zona Hostil
        if (profundidadActual > 8000f) chanceEnemigoReal = 80f; // Zona Mortal (Casi todo son peces)

        // ------------------------------------------

        GameObject prefabAUsar = burbujaPrefab;
        bool esImportante = false; // "Importante" = Dato o Enemigo

        float dado = Random.Range(0f, 100f);

        // --- 2. DECIDIR QUÉ GENERAR ---
        // Usamos 'chanceEnemigoReal' en lugar de la variable fija
        if (dado < probabilidadDato)
        {
            prefabAUsar = datoPrefab;
            esImportante = true;
        }
        else if (dado < probabilidadDato + chanceEnemigoReal)
        {
            prefabAUsar = enemigoPrefab;
            esImportante = true;
        }

        // --- 3. CALCULAR POSICIÓN Y ROTACIÓN ---
        Vector2 posicion;
        Quaternion rotacion;
        float escala;

        if (esImportante)
        {
            // DATOS Y ENEMIGOS: Centrados, Rectos, Tamaño Normal
            // (X=0 para asegurar que el jugador pueda interactuar)
            posicion = new Vector2(0f, -6f);
            rotacion = Quaternion.identity;
            escala = 1f;
        }
        else
        {
            // BURBUJAS: Random X, Rotación en abanico, Tamaño Variado
            float randomX = Random.Range(-2.5f, 2.5f);
            posicion = new Vector2(randomX, -6f);

            float angulo = Random.Range(-30f, 30f);
            rotacion = Quaternion.Euler(0, 0, angulo);

            escala = Random.Range(tamanoMin, tamanoMax);
        }

        // --- 4. CREAR OBJETO ---
        GameObject nuevoObjeto = Instantiate(prefabAUsar, posicion, rotacion);
        nuevoObjeto.transform.localScale = Vector3.one * escala;
    }
}