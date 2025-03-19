using UnityEngine;

public class Bala1 : MonoBehaviour
{
    public float speed = 50f; // Velocidad de la bala
    public float damage; // Daño que inflige la bala
    private Vector3 moveDirection;

    void Start()
    {
        moveDirection = transform.forward; // Dirección inicial de la bala
        float maxDistance = 500f; // Alcance máximo
        float lifeTime = maxDistance / speed; // Calcula el tiempo necesario para recorrer la distancia
        Destroy(gameObject, lifeTime); // Destruir tras alcanzar el alcance máximo
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        RaycastHit hit;

        // Verificar colisiones en el camino
        if (Physics.Raycast(transform.position, moveDirection, out hit, moveDistance))
        {
            OnCollision(hit); // Procesar impacto
        }
        else
        {
            transform.position += moveDirection * moveDistance; // Movimiento de la bala
        }
    }
    public void SetDamage(float damageValue)
    {
        damage = damageValue; // Establecer el daño desde el rifle
    }

    void OnCollision(RaycastHit hit)
    {
        // Detectar si golpea un enemigo
        Enemigo enemigo = hit.collider.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.TakeDamage(damage); // Aplicar daño
        }

        // Destruir tras impacto
        Destroy(gameObject);
    }
}
