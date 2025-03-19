using UnityEngine;

public class Arma1 : MonoBehaviour, IArma
{
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn; // Punto desde donde se dispara el proyectil
    public float damage = 10f; // Da�o que inflige el arma
    public float velocidadProyectil = 20f; // Velocidad a la que se mueve el proyectil
    public void Shoot()
    {
        // Instanciar el proyectil
        GameObject bala = Instantiate(proyectil, transform.position, transform.rotation);

        // Obtener el script "Bala"
        Bala1 proj = bala.GetComponent<Bala1>();
        if (proj != null)
        {
            proj.SetDamage(damage);

            // Configurar la velocidad de la bala
            Vector3 shootDirection = transform.forward; // Direcci�n recta hacia donde mira el objeto
            Rigidbody rb = bala.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = shootDirection * 25f; // Multiplica por la velocidad deseada
            }
            else
            {
                Debug.LogError("El prefab del proyectil no tiene un Rigidbody adjunto.");
            }
        }
        else
        {
            Debug.LogError("El prefab del proyectil no tiene el script Bala adjunto.");
        }
    }



    public float GetDamage()
    {
        return damage;
    }
}
