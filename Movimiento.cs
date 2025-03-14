using UnityEngine;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = -9.81f; // Gravedad est�ndar
    public float mouseSensitivity = 100f;
    public Camera playerCamera;
    public Image cruz; // Referencia a la imagen de la ret�cula
    public float zoomFOV = 40f; // Campo de visi�n al hacer zoom
    public float normalFOV = 60f; // Campo de visi�n normal
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

        // Reproducir la animaci�n Idle al inicio
        animator.Play("Iddle");

        // Asegurar que la ret�cula est� oculta al inicio
        if (cruz != null)
        {
            cruz.enabled = false;
        }
        else
        {
            Debug.LogError("La referencia a la ret�cula (cruz) no est� asignada en el Inspector.");
        }

        // Anclar el rifle a la mano derecha del personaje
        if (rifle != null && rightHand != null)
        {
            rifle.SetActive(true); // Aseg�rate de que el rifle est� activo
            rifle.transform.SetParent(rightHand); // Anclar el rifle a la mano derecha
            rifle.transform.localPosition = new Vector3(-0.039f, 0.067f, 0.011f); // Ajusta seg�n sea necesario                                                                         // Ajusta seg�n sea necesario
            rifle.transform.localRotation = Quaternion.Euler(-106.83f, 77.51f, 14.7f); // Ajusta seg�n sea necesario
        }
        else
        {
            Debug.LogError("El rifle o la mano derecha no est�n asignados en el Inspector.");
        }

        // Ajustar posici�n y rotaci�n del objetivo IK de la mano izquierda
        if (leftHandIKTarget != null)
        {
            leftHandIKTarget.localPosition = new Vector3(0.255f, -0.2f, 0.3f); // Ajusta estos valores seg�n sea necesario
            leftHandIKTarget.localRotation = Quaternion.Euler(30f, 0f, 90f); // Ajusta estos valores seg�n sea necesario
        }
    }

    void Update()
    {
        // Obtener las entradas de movimiento
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Obtener las entradas del rat�n
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ajustar la rotaci�n de la c�mara en el eje vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Ajustar la rotaci�n del personaje en el eje horizontal
        transform.Rotate(Vector3.up * mouseX);

        // Crear direcci�n de movimiento en base a las entradas
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        // Si el personaje est� en el suelo
        if (characterController.isGrounded)
        {
            ySpeed = 0;  // Restablecer la velocidad en Y al estar en el suelo

            // Si presionamos salto, aplicamos el salto
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;  // Aplicar impulso de salto
                animator.SetBool("Salto", true); // Activar animaci�n de salto
            }
            else
            {
                animator.SetBool("Salto", false); // Desactivar animaci�n de salto
            }
           
            
        }
        else
        {
            // Si el personaje no est� en el suelo, aplicar gravedad
            ySpeed += gravity * Time.deltaTime;  // Aplicar la gravedad
        }

        // Crear la velocidad final (solo horizontal y salto)
        Vector3 velocity = moveDirection * speed;  // Movimiento horizontal
        velocity.y = ySpeed;  // Aplicar la velocidad vertical (salto y gravedad)

        // Mover al personaje
        characterController.Move(velocity * Time.deltaTime);

        // Animaci�n: si hay movimiento, activar animaci�n de caminar
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

        // Mano izquierda (ajusta la posici�n al objetivo IK)
        if (leftHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 5f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
    }



}
