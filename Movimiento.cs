using UnityEngine;
using UnityEngine.UI;

public class movimientoSwat : MonoBehaviour
{
    public float speed =3f;
    public float jumpSpeed = 8f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;

    public Camera playerCamera;
    public Image cruz; // Retícula de apuntado
    public GameObject rifle; // Rifle configurado como arma inicial
    public GameObject pistola; // Pistola para cambiar más tarde
    public Transform rightHand; // Mano derecha
    public Transform leftHandIKTarget; // Objetivo IK para la mano izquierda
    public Transform cameraAnchor;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float xRotation = 0f;

    void Start()
    {
        // Configurar referencias
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Configurar el rifle como arma inicial
        if (rifle != null && rightHand != null)
        {
            rifle.SetActive(true); // Activar el rifle
            rifle.transform.SetParent(rightHand); // Anclar el rifle a la mano derecha
            rifle.transform.localPosition = new Vector3(-0.132f, 0.098f, -0.315f); // Ajusta según sea necesario
            rifle.transform.localRotation = Quaternion.Euler(-64.1f, 16.6f, 126.3f); // Ajusta según sea necesario
        }
        else
        {
            Debug.LogError("Rifle o mano derecha no asignados en el Inspector.");
        }

        // Desactivar la pistola al inicio
        if (pistola != null)
        {
            pistola.SetActive(false);
        }

        // Asegurar que la retícula esté oculta al inicio
        if (cruz != null)
        {
            cruz.enabled = false;
        }
        else
        {
            Debug.LogError("No se ha asignado la retícula en el Inspector.");
        }
    }

    void Update()
    {
        // Movimiento y rotación
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Esto es la rotación de la cámara
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -60f, 90f);

        playerCamera.transform.position = cameraAnchor.position;
        playerCamera.transform.rotation = cameraAnchor.rotation;

        transform.Rotate(Vector3.up * mouseX);

        //Se ejecutan los movimientos del personaje
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        if (characterController.isGrounded)
        {
            ySpeed = 0;
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
                animator.SetBool("Salto", true);
            }
            else
            {
                animator.SetBool("Salto", false);
            }
        }
        else
        {
            ySpeed += gravity * Time.deltaTime;
        }

        Vector3 velocity = moveDirection * speed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        // Animaciones de caminar y correr
        animator.SetBool("isWalking", moveDirection.magnitude > 0);

        if (Input.GetButton("LeftShift"))
        {
            speed = 10f;
            animator.SetBool("Correr", true);
        }
        else
        {
            speed = 5f;
            animator.SetBool("Correr", false);
        }


    }

    void OnAnimatorIK(int layerIndex)
    {
        // IK para la mano derecha (rifle)
        if (rightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }
        float leftHandWeight = 1f; // Asegúrate de que este valor esté entre 0 y 1

        if (leftHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }

        // IK para la mano izquierda (apoyo en el rifle)
        if (leftHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }

    }
}
