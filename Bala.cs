using UnityEngine;

public class Bala : MonoBehaviour
{
    public float speed = 20f; // Velocidad de la bala
    public float damage = 5f; // Daño que inflige la bala
    private Vector3 moveDirection; // Dirección de movimiento

    void Start()
    {
        // Configurar la dirección de la bala al crearse
        moveDirection = transform.forward; // La bala se moverá hacia adelante
    }
    public void SetDamage(float damageValue)
    {
        damage = damageValue; // Establecer el daño de la bala
    }
    void Update()
    {
        // Mover la bala manualmente cada frame
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detecta si la bala golpea un enemigo
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.TakeDamage(damage); // Aplica el daño configurado en la bala
        }

        // Destruir la bala tras impactar
        Destroy(gameObject);
    }

}
