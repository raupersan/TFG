using UnityEngine;

public class Disparar1 : MonoBehaviour
{
    public Camera playerCamera;  // Cámara del jugador
    public Transform spawn;      // Punto de aparición del proyectil
    public float rate = 0.5f;    // Cadencia de disparo
    private float shotRate;      // Temporizador para el disparo

    void Update()
    {
        // Detectar si el jugador dispara
        if (Input.GetButton("Fire1") && Time.time > shotRate)
        {
            shotRate = Time.time + rate; // Actualizar temporizador
            DispararArma();
        }
    }

    void DispararArma()
    {
        // Obtener el arma activa
        GameObject armaActual = GestorArmas.Instance.ObtenerArmaActual();

        if (armaActual != null)
        {
            // Obtener el script de la clase del arma activa
            Arma1 scriptArma = armaActual.GetComponent<Arma1>();
            if (scriptArma != null)
            {
                scriptArma.Shoot(); // Llama al método de disparo
            }
        }
        else
        {
            Debug.LogError("No hay arma equipada para disparar.");
        }
    }
}
