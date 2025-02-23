using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = -9.81f; // Gravedad estándar
    public float mouseSensitivity = 100f;
    public Camera playerCamera;
   

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float xRotation = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
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

            // Si mantenemos presionado el botón de apuntar
            if (Input.GetButton("Fire2")) // Clic derecho del ratón por defecto
            {
                animator.SetBool("Apuntar", true); // Activar animación de apuntar
                if (Input.GetButton("Fire1"))
                {
                    animator.SetBool("Disparar",true);
                }
                else
                {
                    animator.SetBool("Disparar", false);
                }
            }
            else
            {
                animator.SetBool("Apuntar", false); // Desactivar animación de apuntar
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
    }
}
