using UnityEngine;

public class Cuchillo : MonoBehaviour
{
    public Transform handTransform; // Transform de la mano del personaje
    public float damage = 5f; // Daño de la pistola
    public Animator animator; // Animator para controlar las animaciones
    public GameObject rifle; // Rifle configurado como arma inicial
    public GameObject pistola; // Pistola para cambiar más tarde
    public GameObject knife; // Cuchillo
    void Start()
    {
        // Anclar la pistola a la mano
        if (handTransform != null && knife != null)
        {
            knife.transform.SetParent(handTransform);
            knife.transform.localPosition = new Vector3(0.037f, 0.07f, 0.041f); // Ajusta según el modelo
            knife.transform.localRotation = Quaternion.Euler(-8.1f, -80.3f, 165.5f); // Ajusta según el modelo

            Debug.Log("Pistola anclada correctamente.");
        }

        // Activar la cuchillo y desactivar el rifle y pistola
        rifle.SetActive(false);
        pistola.SetActive(false);
        knife.SetActive(true);

        // Obtener el Animator
        animator = GameObject.Find("Swat").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el personaje.");
        }
    }

    void Update()
    {
        // Control de la animación de caminar
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("AndarKnife", true); // Activar animación de caminar
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("AndarKnife", false); // Desactivar animación de caminar (debería regresar a "idle")
            }
        }

        // Control de la animación de ataque
        if (Input.GetButton("Fire1")) // Botón izquierdo para atacar
        {
            if (animator != null)
            {
                animator.SetBool("KnifeAttack", true); // Activar animación de ataque
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("KnifeAttack", false); // Desactivar animación de ataque (volver a "idle" o "andar")
            }
        }
    }


}
