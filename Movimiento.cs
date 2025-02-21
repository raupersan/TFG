using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 700f;
    public float jumpSpeed = 8f;
    public float gravity = 0; // Gravedad estándar

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Obtener las entradas de movimiento
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Crear dirección de movimiento en base a las entradas
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Si el personaje está en el suelo
        if (characterController.isGrounded)
        {
            ySpeed = 0;  // Restablecer la velocidad en Y al estar en el suelo

            // Si presionamos salto, aplicamos el salto
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;  // Aplicar impulso de salto
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

        // Rotación suave: si hay movimiento
        if (moveDirection.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);  // Rotación hacia la dirección
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);  // Rotación suave
        }
    }
}
