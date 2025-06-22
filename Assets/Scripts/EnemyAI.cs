using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] patrolPoints;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    bool isDead;

    //Patroling
    Vector3 walkPoint;
    bool walkPointSet;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject shootingPoint;
    public AudioClip shootSFX;
    public AudioClip deathSFX;
    public AudioClip greetingSFX;
    public AudioSource audioSource;


    //States
    public float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;

    private Animator animator;
    private RaycastHit rayHit;

    Transform player;
    CapsuleCollider collider;

    //Patroling
    private int patrolPointIndex = 0;
    public float waitAtPatrolPointTime = 1f;
    private bool isWaiting = false;

    public GameObject muzzleFlash, bulletHoleGraphic, bulletFleshGraphic;

    //States
    public enum State { Patrol, Combat }
    public State currentState = State.Patrol;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        // Start at the first patrol point
        agent.destination = patrolPoints[0].position;
    }

    private void Update()
    {
        if (isDead)
            return;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        //update animations
        if (agent.velocity == Vector3.zero && currentState != State.Combat)
        {
            animator.SetBool("isIdle", true);

        }
        else if (agent.velocity != Vector3.zero && currentState != State.Combat)
        {
            animator.SetBool("isIdle", false);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet && !isWaiting) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f && !isWaiting)
        {
            walkPointSet = false;
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private void SearchWalkPoint()
    {
        walkPoint = patrolPoints[patrolPointIndex].position;
        walkPointSet = true;
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitAtPatrolPointTime);
        isWaiting = false;
        patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
        SearchWalkPoint();
    }

    private void ChasePlayer()
    {
        agent.speed = 5f;
        animator.SetBool("isDetected", true);
        currentState = State.Combat;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here
            Vector3 direction = transform.forward;
            //raycast
            //Debug.DrawRay(shootingPoint.transform.position, direction * 1000, Color.red);

            animator.SetTrigger("Shoot");

            audioSource.clip = shootSFX;
            audioSource.Play();

            

            if (Physics.Raycast(shootingPoint.transform.position, direction, out rayHit, 1000, whatIsPlayer))
            {
                //Debug.Log(rayHit.collider.name);

                if (rayHit.collider.CompareTag("Player"))
                {
                    rayHit.collider.GetComponent<PlayerHealth>().TakeDamage(10);
                    Instantiate(bulletFleshGraphic, rayHit.point, Quaternion.Euler(0, 100, 0));

                }
                else
                {
                    Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 100, 0));
                }
            }
            //End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            animator.SetTrigger("isHit");

            health -= damage;

            if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            audioSource.clip = deathSFX;
            audioSource.clip = greetingSFX;
            audioSource.Play();
            //Destroy(gameObject);
            collider.enabled = false;
            agent.enabled = false;
        }

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }



    
}
