using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float health = 10f;
    private Bala bala;
    public void TakeDamage(float damage)
    {
        health -= damage; // Reducir la salud del enemigo
        Debug.Log($"El enemigo ha recibido {damage} de da�o. Vida restante: {health}");

        if (health <= 0)
        {
            Die(); // Si la salud llega a 0, el enemigo muere
        }
    }


    private void Die()
    {
        Debug.Log("El enemigo ha muerto.");
        // Aqu� puedes agregar efectos de muerte, como una animaci�n o un sonido
        Destroy(gameObject); // Destruye el objeto enemigo
    }
}
