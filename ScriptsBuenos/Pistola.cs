using UnityEngine;

public class Pistola : MonoBehaviour, IArma
{
    public GameObject proyectil; // Prefab de la bala
    public Transform spawn; // Punto donde aparece la bala
    public Transform handTransform; // Transform de la mano del personaje
    public float fuerza = 10000f; // Fuerza de disparo
    public float damage = 10f; // Da�o de la pistola
    public float rate2 = 0.15f; // 10 balas por segundo
    private float shotRate2; // Temporizador para controlar los disparos
    public Animator animator; // Animator para controlar las animaciones
    public Camera playerCamera; // C�mara del jugador (para calcular la direcci�n del disparo)
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public GameObject rifle; // Rifle configurado como arma inicial
    public GameObject pistola; // Pistola para cambiar m�s tarde
    public Transform cameraSpawn; // Spawn para la c�mara cuando se usa la pistola
    public float range = 300000f; // Alcance m�ximo del disparo

    void Start()
    {
        // Anclar la pistola a la mano
        if (handTransform != null && pistola != null)
        {
            pistola.transform.SetParent(handTransform);
            pistola.transform.localPosition = new Vector3(-0.044f, 0.167f, 0.055f); // Ajusta seg�n el modelo
            pistola.transform.localRotation = Quaternion.Euler(16.34f, 333.3f, 90.7f); // Ajusta seg�n el modelo

            Debug.Log("Pistola anclada correctamente.");
        }

        // Activar la pistola y desactivar el rifle
        rifle.SetActive(false);
        pistola.SetActive(true);
        // Configurar la c�mara en el spawn de la pistola
        if (cameraSpawn != null && playerCamera != null)
        {
            playerCamera.transform.position = cameraSpawn.position;
            playerCamera.transform.rotation = cameraSpawn.rotation;
        }
        // Obtener el Animator
        animator = GameObject.Find("Swat").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� un Animator en el personaje.");
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire2")) // Bot�n derecho para apuntar
        {
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Time.time >= shotRate2))
            {
                shotRate2 = Time.time + rate2; // Actualizar el temporizador
                Shoot();
            }
            // Actualizar la posici�n de la c�mara en tiempo real
            if (cameraSpawn != null)
            {
                playerCamera.transform.position = cameraSpawn.position;
                playerCamera.transform.rotation = cameraSpawn.rotation;
            }
        }
        else
        {
            animator.SetBool("DispararPistola", false);
        }
    }

    public void Shoot()
    {
        if (animator != null)
        {
            animator.SetBool("DispararPistola", true);

            // Raycast desde el centro de la c�mara
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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

            // Configurar el da�o de la bala
            Bala1 balaScript = bala.GetComponent<Bala1>();
            if (balaScript != null)
            {
                balaScript.SetDamage(damage);
                Debug.Log("Da�o configurado en la bala: " + damage);
            }
            else
            {
                Debug.LogError("El script Bala1 no est� asignado al prefab de la bala.");
            }

            Debug.Log("�Disparo realizado!");
        }
        else
        {
            Debug.LogError("El Animator no est� asignado.");
        }
    }
}
