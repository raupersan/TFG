using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    public float health = 10f;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks = 2f;
    bool alreadyAttacked;
    public GameObject projectile;

    // States
    public float rangoAlcance = 10f;
    public float rangoAtaque = 5f;
    public bool jugadorEnRangoDeAlcance, jugadorEnRangoDeAtaque;

    private void Awake()
    {
        player = GameObject.Find("Swat").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if player is within detection range
        jugadorEnRangoDeAlcance = Physics.CheckSphere(transform.position, rangoAlcance, whatIsPlayer);
        jugadorEnRangoDeAtaque = Physics.CheckSphere(transform.position, rangoAtaque, whatIsPlayer);

        if (!jugadorEnRangoDeAlcance && !jugadorEnRangoDeAtaque)
            Patrolling();
        if (jugadorEnRangoDeAlcance && !jugadorEnRangoDeAtaque)
            ChasePlayer();
        if (jugadorEnRangoDeAlcance && jugadorEnRangoDeAtaque)
            AttackPlayer();
    }

    public void TakeDamage(float damage)
    {
        health -= damage; // Reducir la salud del enemigo
        Debug.Log($"El enemigo ha recibido {damage} de daño. Vida restante: {health}");

        if (health <= 0)
        {
            Die(); // Si la salud llega a 0, el enemigo muere
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Encuentra un nuevo punto de patrullaje
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Verificar si el punto está en el suelo
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("Persiguiendo al jugador.");
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        Debug.Log("Iniciando ataque.");
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 1.5f + Vector3.up * 1f, Quaternion.identity).GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("El proyectil no tiene un componente Rigidbody.");
            return;
        }

        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        Debug.Log("Proyectil lanzado hacia el jugador.");

        // Verificar impacto
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rangoAtaque))
        {
            Debug.Log($"Impacto en: {hit.collider.name}");
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<VidaPersonaje>()?.TakeDamage(10f);
            }
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Die()
    {
        Debug.Log("El enemigo ha muerto.");
        Destroy(gameObject); // Destruye el objeto enemigo
    }
}
