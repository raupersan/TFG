using UnityEngine;

public class Bala1 : MonoBehaviour
{
    public float speed = 50f; // Velocidad de la bala
    public float damage; // Daño que inflige la bala
    private Vector3 moveDirection;

    void Start()
    {
        moveDirection = transform.forward; // Dirección inicial de la bala
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime; // Movimiento de la bala
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue; // Establecer el daño desde el rifle
    }

    void OnCollisionEnter(Collision collision)
    {
        // Procesar colisión con enemigos u objetos
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.TakeDamage(damage); // Aplicar daño
        }

        // Destruir tras impacto
        Destroy(gameObject);
    }
}

//Va con Rifle1.cs solamente (por ahora). Y lo mismo que el rifle falta mirarle lo de las balas
