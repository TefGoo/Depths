using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Configuración Básica")]
    public GameObject objetoPrefab;
    public float tiempoEntreSpawns = 1f;

    [Header("Variación (El Sabor)")]
    public float tamanoMin = 0.5f; // 50% del tamaño original
    public float tamanoMax = 1.5f; // 150% del tamaño original
    public bool rotarAleatorio = true; // Casilla para activar/desactivar rotación

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
        // 1. Posición X aleatoria (ajusta el -2.5f y 2.5f al ancho de tu pantalla)
        float randomX = Random.Range(-2.5f, 2.5f);
        Vector2 posicionNacimiento = new Vector2(randomX, -6f);

        // 2. Rotación: ¿Quieres que roten o que salgan derechas?
        Quaternion rotacion = Quaternion.identity; // Derecha por defecto
        if (rotarAleatorio)
        {
            float anguloAbanico = Random.Range(-35f, 35f);
            rotacion = Quaternion.Euler(0, 0, anguloAbanico);
        }

        // 3. Crear el objeto (guardamos una referencia en 'nuevoObjeto' para modificarlo)
        GameObject nuevoObjeto = Instantiate(objetoPrefab, posicionNacimiento, rotacion);

        // 4. CAMBIO DE TAMAÑO (Aquí está la magia)
        float escalaAleatoria = Random.Range(tamanoMin, tamanoMax);
        nuevoObjeto.transform.localScale = Vector3.one * escalaAleatoria;
        // Vector3.one es (1,1,1). Al multiplicar por 0.5 queda (0.5, 0.5, 0.5)
    }
}