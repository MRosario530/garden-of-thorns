using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearController : Enemy
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround, targetLayerMask;
    public Animator animator;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public float timeBetweenPatrols = 2f;

    // Attacking
    public float timeBetweenAttacks;
    public int damage = 20;
    
    [SerializeField] private AudioClip chasingSoundClip;
    [SerializeField] private AudioClip attackingSoundClip;
    [SerializeField] private AudioClip deathSoundClip;

    // States
    public float sightRange, attackRange, chaseRange;
    public bool isChasing = false;
    public bool canPatrol = true;
    public bool canAttack = true;
    public bool targetInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!isAlive)
        {
            return;
        }

        if (target != null && target.gameObject.layer == LayerMask.NameToLayer("Inactive"))
        {
            target = null;
        }

        if (target == null)
        {
            // Find a target
            Collider[] targets = Physics.OverlapSphere(transform.position, sightRange, targetLayerMask);

            float minDistance = Mathf.Infinity;
            Transform nearestTarget = null;

            foreach (Collider col in targets)
            {

                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestTarget = col.transform;
                }
            }

            target = nearestTarget;
        }

        if (target != null)
        {
            targetInAttackRange = Vector3.Distance(transform.position, target.position) <= attackRange;

            if (!isChasing)
            {
                AudioSource.PlayClipAtPoint(chasingSoundClip, transform.position);
                isChasing = true;
            }

            if (targetInAttackRange)
            {
                StartCoroutine(Attack());
            }
            else
            {
                Chase();
            }
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    private void Patrol()
    {
        agent.speed = walkSpeed;

        if (!walkPointSet && canPatrol)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (walkPointSet)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                walkPointSet = false;
                canPatrol = false;
                Invoke(nameof(ResetPatrol), timeBetweenPatrols);
                animator.SetBool("WalkForward", false);
                animator.SetBool("Idle", true);
            }
            else
            {
                animator.SetBool("WalkForward", true);
            }
        }
    }

    private void ResetPatrol()
    {
        canPatrol = true;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomPointInUnitCircle2D = Random.insideUnitCircle;
            Vector3 randomPointInUnitCircle3D = new Vector3(randomPointInUnitCircle2D.x, 0, randomPointInUnitCircle2D.y);
            Vector3 randomPoint = center + randomPointInUnitCircle3D * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void SearchWalkPoint()
    {
        animator.SetBool("WalkForward", true);
        animator.SetBool("Idle", false);

        float range = 5f;
        if (RandomPoint(transform.position, range, out walkPoint))
        {
            walkPointSet = true;
            Debug.DrawRay(walkPoint, Vector3.up, Color.blue, 1.0f);
        }
    }

    private void Chase()
    {
        agent.speed = runSpeed;
        Vector3 newPosition = target.position + (transform.position - target.position).normalized * chaseRange;
        agent.SetDestination(newPosition);
        animator.SetBool("WalkForward", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Run Forward", true);
    }

    private IEnumerator Attack()
    {
        animator.SetBool("Run Forward", false);
        animator.SetBool("Idle", false);
        transform.LookAt(target);

        if (canAttack)
        {
            if (targetInAttackRange)
            {
                animator.SetTrigger("Attack1");
                canAttack = false;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                AudioSource.PlayClipAtPoint(attackingSoundClip, transform.position);

                yield return new WaitForSeconds(0.4f);

                if (!isAlive || target == null)
                {
                    yield break;
                }

                target.GetComponent<IFriendlyTakeDamage>().TakeDamage(damage);
            }
        }   
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    override
    public void TakeDamage(int damage, Transform attacker = null)
    {   
        if (!isAlive)
        {
            return;
        }

        SetHealth(currentHealth - damage);
        animator.SetTrigger("Get Hit Front");

        if (target == null && attacker != null)
        {
            target = attacker;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            Invoke(nameof(OnDeath), .1f);
        }
    }

    override
    public void OnDeath()
    {
        animator.SetBool("WalkForward", false);
        animator.SetBool("Run Forward", false);
        animator.SetBool("Death", true);
        target = null;
        agent.ResetPath();

        gameObject.layer = LayerMask.NameToLayer("Inactive");

        if (roomController)
        {
            roomController.EnemyDefeated();
        }

        float deathDelay = 3.5f;
        Invoke(nameof(DestroyEnemyDelayed), deathDelay);
        AudioSource.PlayClipAtPoint(deathSoundClip, transform.position);
    }

    private void DestroyEnemyDelayed()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}