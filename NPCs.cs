using UnityEngine;
using UnityEngine.AI;
//del
//https://www.youtube.com/watch?v=UjkSFoLxesw
public class EnemyAITutorial : MonoBehavious
{
    public NavMeshAgent agent;
    public float health
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint; 

    //Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float rangoAlcance, rangoAtaque;
    public bool jugadorEnRangoDeAlcance, jugadorEnRangoDeAtaque;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update ()
    {
        jugadorEnRangoDeAlcance = Physics.CheckSphere(transform.position, rangoAlcance, whatIsPlayer);
        jugadorEnRangoDeAtaque = Physics.CheckSphere(transform.position, rangoAtaque, whatIsPlayer);
    if(!jugadorEnRangoDeAlcance && !jugadorEnRangoDeAtaque)
        Patrolling();
    if(jugadorEnRangoDeAlcance && !jugadorEnRangoDeAtaque)
        ChasePlayer();
    if(jugadorEnRangoDeAlcance && jugadorEnRangoDeAtaque)
        AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet =false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + transform.position.z + randomZ)

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
     private void ChasePlayer()
    {
        agent.SetDestination(player.position);

    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //Attack code
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse)
            rb.AddForce(transform.up * 8f, ForceMode.Impulse)

            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
   private void ResetAttack()
   {
    alreadyAttacked = false;

   }
    public int TakeDamage()
    {
        health -= damage;
        if(health<=0)
            Invoke(nameof(DestroyEnemy), .5f);
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject)
    }
}
