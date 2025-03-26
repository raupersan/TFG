using UnityEngine;

public class Pistola : MonoBehaviour, IArma
{
    [Header("Configuración de la Pistola")]
    public GameObject proyectil; // Prefab de la bala
    public Transform spawn; // Punto donde aparece la bala
    public Transform handTransform; // Transform de la mano del personaje
    public float fuerza = 10000f; // Fuerza de disparo
    public float damage = 10f; // Daño de la pistola
    public float rate2 = 1f; // 1 bala por segundo
    private float shotRate2; // Temporizador para controlar los disparos
    public Animator animator; // Animator para controlar las animaciones
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public float range = 300f; // Alcance máximo del disparo
    public GameObject pistola; // Pistola para cambiar más tarde
    public GameObject knife; // Cuchillo
    public GameObject rifle; // Cuchillo
    void Start()
    {
        // Anclar la pistola a la mano
        if (handTransform != null && gameObject != null)
        {
            pistola.transform.SetParent(handTransform);
            pistola.transform.localPosition = new Vector3(-0.044f, 0.167f, 0.055f); // Ajusta según el modelo
            pistola.transform.localRotation = Quaternion.Euler(35.5f, -26.7f, 90.7f); // Ajusta según el modelo

            Debug.Log("Pistola anclada correctamente a la mano.");
        }

        // Activar la pistola al iniciar
        pistola.SetActive(true);
        knife.SetActive(false);
        rifle.SetActive(false);
        // Obtener el Animator
        animator = GameObject.Find("Swat").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el personaje.");
        }
    }

    void Update()
    {
        // Lógica de caminar (animación)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("AndarPistola", true); // Activar animación de caminar
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("AndarPistola", false); // Desactivar animación de caminar
            }
        }

        // Lógica de disparo
        if (Input.GetButton("Fire2")) // Botón derecho para apuntar
        {
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Time.time >= shotRate2))
            {
                shotRate2 = Time.time + rate2; // Actualizar el temporizador
                Shoot(); // Llamar al método de disparo
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("DispararPistola", false); // Desactivar la animación de disparo
            }
        }
    }

    public void Shoot()
    {
        if (animator != null)
        {
            animator.SetBool("DispararPistola", true); // Activar animación de disparo
        }

        // Raycast desde el centro de la cámara
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Usar la cámara principal
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, range, aimLayerMask))
        {
            targetPoint = hit.point; // Punto de impacto del raycast
        }
        else
        {
            targetPoint = ray.GetPoint(range); // Punto lejano si no impacta nada
        }

        // Crear la bala
        Vector3 direction = (targetPoint - spawn.position).normalized;
        GameObject bala = Instantiate(proyectil, spawn.position, Quaternion.LookRotation(direction));

        // Configurar el daño de la bala
        Bala1 balaScript = bala.GetComponent<Bala1>();
        if (balaScript != null)
        {
            balaScript.SetDamage(damage);
            Debug.Log("Daño configurado en la bala: " + damage);
        }
        else
        {
            Debug.LogError("El script Bala1 no está asignado al prefab de la bala.");
        }

        Debug.Log("¡Disparo realizado!");
    }
}
