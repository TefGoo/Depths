using UnityEngine;

public class Recolectable : MonoBehaviour
{
    public int valor = 1; // Cuánto dinero da

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con el Jugador...
        if (other.CompareTag("Player"))
        {
            // 1. Dar dinero (Crearemos esta función en un segundo)
            GameManager.instance.AddMoney(valor);

            // 2. Desaparecer
            Destroy(gameObject);
        }
    }
}