using UnityEngine;
using UnityEngine.UI;

public class Apuntado1 : MonoBehaviour
{
    public Animator animator;    // Animator del personaje para controlar animaciones
    public Image cruz;           // Retícula en el UI (asignada desde el Canvas)
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto de aparición del proyectil
    public Camera playerCamera;  // Cámara del jugador
    public float rate = 0.5f;    // Cadencia de disparo
    private float shotRate;      // Temporizador para controlar los disparos
    public float damage = 20f;   // Daño del arma
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public Transform cameraAimPoint; // Punto en el rifle hacia donde la cámara debe apuntar

    void Start()
    {

        //// Ocultar retícula al inicio
        if (cruz != null)
        {
            cruz.enabled = false; // La retícula comienza oculta
        }
        else
        {
            Debug.LogError("No se ha asignado la retícula (cruz) en el Inspector.");
        }
    }

    public void Update()
    {
        if (cameraAimPoint != null && playerCamera != null)
        {
            // Ajuste de la posición y rotación de la cámara
            Vector3 offset = new Vector3(0, 0.3f, -0.5f); // Ajuste para evitar colisiones con objetos cercanos
            playerCamera.transform.position = cameraAimPoint.position + cameraAimPoint.forward * offset.z + cameraAimPoint.up * offset.y;

            Quaternion targetRotation = cameraAimPoint.rotation;
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0f);
            playerCamera.transform.rotation = targetRotation;
        }

        // Control del apuntado (Fire2)
        if (Input.GetButton("Fire2")) // Botón derecho para apuntar
        {
            // Mostrar la retícula
            if (cruz != null)
            {
                cruz.enabled = true;
                cruz.rectTransform.sizeDelta = new Vector2(32, 32); // Tamaño fijo de la retícula
            }

            // Reducir FOV para zoom
            if (playerCamera != null)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 45f, Time.deltaTime * 5f); // Reducir FOV
            }
        }
        else
        {
            // Ocultar la retícula
            if (cruz != null)
            {
                cruz.enabled = false;
            }

            // Restaurar FOV al valor normal
            if (playerCamera != null)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 60f, Time.deltaTime * 5f);
            }
        }
    }

}


