using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Enemy
{
    public NavMeshAgent agent;
    public float moveSpeed = 3.5f;
    public LayerMask whatIsGround, targetLayerMask;
    public new ParticleSystem particleSystem;

    // Attacking
    public float timeBetweenAttacks;
    bool canAttack;
    public GameObject projectile;

    // Gun
    [SerializeField] Transform gun;
    Vector3 gunDirection;
    float gunLength;
    Vector3 gunTipPosition;

    // States
    public float sightRange, attackRange, chaseRange;
    public bool targetInAttackRange;

    [Header("Audio")]
    [SerializeField] private AudioClip shootingSoundClip;
    AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        canAttack = true;

        // Initialize health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    private void Update()
    {
        gunDirection = gun.forward;
        gunLength = Vector3.Distance(gun.position, gun.TransformPoint(Vector3.forward * gun.localScale.z));
        gunTipPosition = gun.position + gunDirection * gunLength;

        if (target != null && target.gameObject.layer == LayerMask.NameToLayer("Inactive"))
        {
            target = null;
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

        if (agent.velocity.magnitude > 0.1f)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
    }

    private void Chase()
    {
        Vector3 newPosition = target.position + (transform.position - target.position).normalized * chaseRange;
        agent.SetDestination(newPosition);
    }

    private void Attack()
    {
        transform.LookAt(target);

        if (canAttack)
        {
            // Attack code here
            GameObject proj = Instantiate(projectile, gunTipPosition, Quaternion.identity);
            Destroy(proj, 5);
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            audioSrc.PlayOneShot(shootingSoundClip);

            rb.AddForce(transform.forward * 40f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);

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