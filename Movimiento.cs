using UnityEngine;
using UnityEngine.EventSystems;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 700f;
    public float jumpSpeed = 8f;

    public Animator animator;
    public CharacterController characterController;
    private float ySpeed;

    public float moveSpeed = 3.0f;  // Velocidad de movimiento
    public float lookSpeedX = 2.0f;  // Velocidad de rotación horizontal
    public float lookSpeedY = 2.0f;  // Velocidad de rotación vertical

    private Camera playerCamera;
    private float rotationX = 0;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();  // Obtén la cámara del jugador
        characterController = GetComponent<CharacterController>();  // Obtén el CharacterController
        Cursor.lockState = CursorLockMode.Locked;  // Oculta el cursor y lo bloquea en el centro
        Cursor.visible = false;
    }

    void Update()
    {
        // Movimiento del jugador
        float moveDirectionX = Input.GetAxis("Horizontal") * moveSpeed; // Movimiento horizontal (A/D o flechas)
        float moveDirectionZ = Input.GetAxis("Vertical") * moveSpeed;   // Movimiento vertical (W/S o flechas)
        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;
        Vector3 velocity = move * speed;

        // Mueve al jugador
        characterController.Move(move * Time.deltaTime);

        characterController.Move(velocity * Time.deltaTime);

        // Animación de caminar
        animator.SetBool("isWalking", move.magnitude > 0);

        // Rotación del personaje
        if (move.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
