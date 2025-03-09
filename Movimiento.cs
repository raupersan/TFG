using UnityEngine;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = -9.81f; // Gravedad estándar
    public float mouseSensitivity = 100f;
    public Camera playerCamera;
    public Image cruz; // Referencia a la imagen de la retícula
    public float zoomFOV = 40f; // Campo de visión al hacer zoom
    public float normalFOV = 60f; // Campo de visión normal
    public float zoomSpeed = 10f; // Velocidad del zoom
    public GameObject rifle; // Asigna el rifle en el Inspector
    public Transform rightHand; // Asigna la mano derecha del personaje en el Inspector
    public Transform leftHandIKTarget; // Asigna el objetivo IK de la mano izquierda en el Inspector

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float xRotation = 0f;

    public Arma arma;

    public float runSpeed = 10f;
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla

        // Reproducir la animación Idle al inicio
        animator.Play("Iddle");

        // Asegurar que la retícula esté oculta al inicio
        if (cruz != null)
        {
            cruz.enabled = false;
        }
        else
        {
            Debug.LogError("La referencia a la retícula (cruz) no está asignada en el Inspector.");
        }

        // Anclar el rifle a la mano derecha del personaje
        if (rifle != null && rightHand != null)
        {
            rifle.SetActive(true); // Asegúrate de que el rifle esté activo
            rifle.transform.SetParent(rightHand); // Anclar el rifle a la mano derecha
            rifle.transform.localPosition = new Vector3(-0.039f, 0.067f, 0.011f); // Ajusta según sea necesario                                                                         // Ajusta según sea necesario
            rifle.transform.localRotation = Quaternion.Euler(-106.83f, 77.51f, 14.7f); // Ajusta según sea necesario
        }
        else
        {
            Debug.LogError("El rifle o la mano derecha no están asignados en el Inspector.");
        }

        // Ajustar posición y rotación del objetivo IK de la mano izquierda
        if (leftHandIKTarget != null)
        {
            leftHandIKTarget.localPosition = new Vector3(0.255f, -0.2f, 0.3f); // Ajusta estos valores según sea necesario
            leftHandIKTarget.localRotation = Quaternion.Euler(30f, 0f, 90f); // Ajusta estos valores según sea necesario
        }
    }

    void Update()
    {
        // Obtener las entradas de movimiento
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Obtener las entradas del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ajustar la rotación de la cámara en el eje vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Ajustar la rotación del personaje en el eje horizontal
        transform.Rotate(Vector3.up * mouseX);

        // Crear dirección de movimiento en base a las entradas
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        // Si el personaje está en el suelo
        if (characterController.isGrounded)
        {
            ySpeed = 0;  // Restablecer la velocidad en Y al estar en el suelo

            // Si presionamos salto, aplicamos el salto
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;  // Aplicar impulso de salto
                animator.SetBool("Salto", true); // Activar animación de salto
            }
            else
            {
                animator.SetBool("Salto", false); // Desactivar animación de salto
            }
           
            
        }
        else
        {
            // Si el personaje no está en el suelo, aplicar gravedad
            ySpeed += gravity * Time.deltaTime;  // Aplicar la gravedad
        }

        // Crear la velocidad final (solo horizontal y salto)
        Vector3 velocity = moveDirection * speed;  // Movimiento horizontal
        velocity.y = ySpeed;  // Aplicar la velocidad vertical (salto y gravedad)

        // Mover al personaje
        characterController.Move(velocity * Time.deltaTime);

        // Animación: si hay movimiento, activar animación de caminar
        animator.SetBool("isWalking", moveDirection.magnitude > 0);

        if (Input.GetButton("LeftShift")) // Detects if the button is being held down
        {
            speed = 10f;
            animator.SetBool("Correr", true); // Activates the running animation
        }
        else
        {
            speed = 3f;
            animator.SetBool("Correr", false); // Stops the running animation
        }


    }
    void ApuntarArma()
    {
        // Realizar un Raycast desde el centro de la cámara hacia adelante
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Centro de la pantalla
        RaycastHit hit;

        // Dibujar el raycast para depuración (esto aparece en la ventana de Scene)
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        // Determinar el punto objetivo
        Vector3 puntoObjetivo;
        if (Physics.Raycast(ray, out hit, 100f)) // Si el Raycast golpea algo
        {
            puntoObjetivo = hit.point;
        }
        else // Si no golpea nada, apunta a un punto lejos en el eje Z
        {
            puntoObjetivo = ray.GetPoint(100f); // A 100 unidades hacia adelante
        }

        // Calcular la rotación necesaria para que el arma apunte al punto objetivo
        Quaternion rotacionObjetivo = Quaternion.LookRotation(puntoObjetivo - rifle.transform.position);

        // Ajustar suavemente la rotación usando Slerp
        rifle.transform.rotation = Quaternion.Slerp(rifle.transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
    }





    void OnAnimatorIK(int layerIndex)
    {
        // Mano derecha (arma conectada a la mano derecha)
        if (rightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }

        // Mano izquierda (ajusta la posición al objetivo IK)
        if (leftHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 5f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
    }



}
