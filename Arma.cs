using UnityEngine;

public class Arma : MonoBehaviour, IArma
{
    public float damage = 10f; //Daño que hace
    public GameObject proyectil; // Prefab de las balas

    public void Shoot()
    {
        GameObject bala = Instantiate(proyectil, transform.position, transform.rotation);
        Bala proj = bala.GetComponent<Bala>();
        if (proj != null)
        {
            proj.SetDamage(damage);

            // Configurar la velocidad de la bala
            proj.speed = 25f; // Cambia este valor para personalizar la velocidad
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
