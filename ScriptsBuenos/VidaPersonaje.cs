using UnityEngine;
using UnityEngine.UI;

public class VidaPersonaje : MonoBehaviour
{
    public float maxHealth = 100f; // Vida máxima
    public float currentHealth;   // Vida actual
    public Slider healthSlider;   // Barra de vida en el UI. Parte de Nati Natasha :3

    void Start()
    {
        // Inicializar vida
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        // Reducir salud
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Actualizar barra de vida
        }

        Debug.Log($"Jugador recibió {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // Incrementar salud
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Actualizar barra de vida
        }

        Debug.Log($"Jugador se curó {amount} puntos. Vida actual: {currentHealth}");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"El jugador ha tocado: {other.name}");

        if (other.CompareTag("Enemy")) // Verifica si es un enemigo
        {
            Debug.Log("El jugador fue golpeado por un enemigo.");
            TakeDamage(10f); // Aplicar 10 de daño (ajusta según sea necesario)
        }
    }
    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        Destroy(gameObject);
    }
}
//Aviso a navegantes: hay que asignarle una barra de vida y mejorar esto en general. El jugador tiene que tener un mesh collider y rigid body
