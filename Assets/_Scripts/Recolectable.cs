using UnityEngine;

public class Recolectable : MonoBehaviour
{
    public int valor = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ahora llama a AddMoney (que suma a la bolsa temporal)
            if (GameManager.instance != null)
            {
                GameManager.instance.AddMoney(valor);
            }

            Destroy(gameObject);
        }
    }
}