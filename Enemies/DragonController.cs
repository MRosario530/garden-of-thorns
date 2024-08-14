using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DragonController : Enemy
{
    public NavMeshAgent agent;
    public LayerMask targetLayerMask;
    [SerializeField] new ParticleSystem particleSystem;

    // Attacking
    public float timeBetweenAttacks;
    bool canAttack;
    public GameObject projectile;
    public AudioSource audioSource;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkSpeed;
    public float runSpeed;
    public float timeBetweenPatrols;
    public bool canPatrol;

    // States
    public float sightRange, attackRange, chaseRange;
    public bool targetInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        agent.speed = walkSpeed;
        walkPointSet = false;
        canAttack = true;
        canPatrol = true;

        // Initialize health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    private void Update()
    {
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

            Chase();

            if (targetInAttackRange)
            {
                Attack();
            }
        }
        else
        {
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
            }
        }
    }

    private void ResetPatrol()
    {
        canPatrol = true;
    }

    private void SearchWalkPoint()
    {
        float range = 5f;
        if (RandomPoint(transform.position, range, out walkPoint))
        {
            walkPointSet = true;
            Debug.DrawRay(walkPoint, Vector3.up, Color.blue, 1.0f);
        }
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

    private void Chase()
    {
        agent.speed = runSpeed;
        Vector3 newPosition = target.position + (transform.position - target.position).normalized * chaseRange;
        agent.SetDestination(new Vector3(newPosition.x, transform.position.y, newPosition.z));
    }

    private void Attack()
    {
        transform.LookAt(target);
        if (canAttack)
        {
            ParticleSystem firebreath = Instantiate(particleSystem, transform);
            firebreath.Play();
            audioSource.Play();

            canAttack = false;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    override
    public void OnDeath()
    {
        if (roomController)
        {
            roomController.EnemyDefeated();
        }
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
