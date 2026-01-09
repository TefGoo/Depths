using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // Ya no necesitamos la variable 'damage' aquí, la controla el GameManager

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // LEER EL DAÑO GLOBAL DESDE EL GAMEMANAGER
                int danoActual = GameManager.instance.collisionDamage;

                GameManager.instance.TakeDamage(danoActual);
            }

            Destroy(gameObject);
        }
    }
}