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

    private void AttackPlayer()
    {
        // Detén al enemigo y haz que mire hacia el jugador
        agent.SetDestination(transform.position);

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (!alreadyAttacked)
        {
            // Código de ataque
            Rigidbody rb = Instantiate(projectile, transform.position + Vector3.up * 1.5f, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 15f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);

            Debug.Log("El enemigo está atacando.");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
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
