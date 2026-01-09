using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int damage = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Esta línea imprimirá TODO lo que toque el pez en la consola
        Debug.Log("El enemigo chocó con: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("¡ATAQUE AL JUGADOR CONFIRMADO!");
            GameManager.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}