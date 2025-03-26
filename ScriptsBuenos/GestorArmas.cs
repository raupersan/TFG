using UnityEngine;

public class GestorArmas : MonoBehaviour
{
    [Header("Configuración de Armas")]
    public GameObject pistola; // GameObject de la pistola
    public GameObject rifle;   // GameObject del rifle
    public GameObject knife;   // GameObject del cuchillo

    [Header("Animaciones")]
    public Animator animator;   // Animator para controlar las animaciones

    private GameObject armaActual; // Arma actualmente equipada

    public static GestorArmas Instance { get; private set; } // Singleton

    void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Más de un GestorArmas encontrado en la escena.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Configurar el arma inicial (por ejemplo, el rifle)
        if (rifle != null)
        {
            CambiarArma(rifle);
            Debug.Log("Rifle configurado como arma inicial.");
        }
        else
        {
            Debug.LogError("El rifle no está asignado en el Inspector.");
        }
    }

    void Update()
    {
        // Cambiar a pistola al presionar la tecla 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CambiarArma(pistola);
            animator.SetBool("Pistola", true);  // Activar animación de pistola
            animator.SetBool("Knife", false);   // Desactivar animación de cuchillo
        }
        // Cambiar a rifle al presionar la tecla 2
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CambiarArma(rifle);
            animator.SetBool("Pistola", false); // Desactivar animación de pistola
            animator.SetBool("Knife", false);   // Desactivar animación de cuchillo
        }
        // Cambiar a cuchillo al presionar la tecla 0
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (knife != null)
            {
                CambiarArma(knife);
                animator.SetBool("Pistola", false); // Desactivar animación de pistola
                animator.SetBool("Knife", true);    // Activar animación de cuchillo
            }
            else
            {
                Debug.LogError("El cuchillo no está asignado en el Inspector.");
            }
        }
    }

    public void CambiarArma(GameObject nuevaArma)
    {
        // Desactivar el arma actual
        if (armaActual != null)
        {
            armaActual.SetActive(false);
            Debug.Log($"Arma anterior desactivada: {armaActual.name}");
        }

        // Activar la nueva arma
        if (nuevaArma != null)
        {
            nuevaArma.SetActive(true);
            armaActual = nuevaArma; // Actualizar referencia al arma actual

            Debug.Log($"Nueva arma activada: {armaActual.name}");
        }
        else
        {
            Debug.LogError("El objeto del arma es nulo.");
        }
    }

    public GameObject ObtenerArmaActual()
    {
        return armaActual;
    }
}
