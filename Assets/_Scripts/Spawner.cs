using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject burbujaPrefab;
    public GameObject datoPrefab;
    public GameObject enemigoPrefab;

    [Header("Configuración Base")]
    public float tiempoEntreSpawns = 0.5f;
    [Range(0, 100)] public float probabilidadDato = 10f;
    [Range(0, 100)] public float probabilidadEnemigo = 15f;

    [Header("Variación Burbujas")]
    public float tamanoMin = 0.5f;
    public float tamanoMax = 1.5f;

    private float tiempoSiguiente = 0f;

    void Update()
    {
        if (GameManager.instance == null) return;

        // 1. ZONA SEGURA (0m) -> No sale nada
        if (GameManager.instance.currentDepth <= 0) return;

        if (Time.time > tiempoSiguiente)
        {
            GenerarObjeto();
            tiempoSiguiente = Time.time + tiempoEntreSpawns;
        }
    }

    void GenerarObjeto()
    {
        GameObject prefabAUsar = burbujaPrefab; // Por defecto es burbuja
        bool esImportante = false;

        // --- 1. FILTRO DE PÁNICO ---
        // Solo calculamos si sale Dato o Enemigo si NO estamos huyendo.
        // Si estamos huyendo (isReturning es true), saltamos este bloque y se queda como burbuja.
        if (!GameManager.instance.isReturning)
        {
            // --- CÁLCULO DE DIFICULTAD ---
            float profundidadActual = GameManager.instance.currentDepth;
            float chanceEnemigoReal = probabilidadEnemigo;
            if (chanceEnemigoReal <= 0) chanceEnemigoReal = 10f;

            if (profundidadActual > 2000f) chanceEnemigoReal = 30f;
            if (profundidadActual > 5000f) chanceEnemigoReal = 50f;
            if (profundidadActual > 8000f) chanceEnemigoReal = 80f;

            // --- TIRAR DADO ---
            float dado = Random.Range(0f, 100f);

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
        }

        // --- 2. POSICIÓN (SUELO O TECHO) ---
        float alturaSpawn = -6f;

        if (GameManager.instance.isReturning)
        {
            alturaSpawn = 6f; // Si huye, nace en el techo
        }

        // --- 3. INSTANCIAR ---
        Vector2 posicion;
        Quaternion rotacion;
        float escala;

        if (esImportante)
        {
            // DATOS Y ENEMIGOS (Solo pasará aquí si NO estamos huyendo)
            posicion = new Vector2(0f, alturaSpawn);
            rotacion = Quaternion.identity;
            escala = 1f;
        }
        else
        {
            // BURBUJAS (Siempre pasa por aquí al huir)
            float randomX = Random.Range(-2.5f, 2.5f);
            posicion = new Vector2(randomX, alturaSpawn);

            float angulo = Random.Range(-30f, 30f);
            rotacion = Quaternion.Euler(0, 0, angulo);
            escala = Random.Range(tamanoMin, tamanoMax);
        }

        GameObject nuevoObjeto = Instantiate(prefabAUsar, posicion, rotacion);
        nuevoObjeto.transform.localScale = Vector3.one * escala;
    }
}