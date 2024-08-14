using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AllyController : Friendly
{
    public NavMeshAgent agent;
    public Transform enemy, player;
    public LayerMask whatIsGround, whatIsEnemy, whatIsPlayer;
    public ParticleSystem movementParticleSystem;
    public ParticleSystem healingParticleSystem;

    public float allySpeed = 5f;

    // Patroling
    public float distanceBehindPlayer = 2f;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    [SerializeField] private AudioClip shootingSoundClip;

    // Gun
    [SerializeField] Transform gun;
    Vector3 gunDirection;
    float gunLength;
    Vector3 gunTipPosition;

    // States
    public float sightRange, attackRange;
    public bool enemyInSightRange, enemyInAttackRange, allyRecharge;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = allySpeed;
        allyRecharge = false;
    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        healthbar.SetMaxHealth(maxHealth);
        SetHealth(maxHealth);
    }

    private void Update()
    {
        gunDirection = gun.forward;
        gunLength = Vector3.Distance(gun.position, gun.TransformPoint(Vector3.forward * gun.localScale.z));
        gunTipPosition = gun.position + gunDirection * gunLength;

        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, whatIsEnemy);   
        if (enemies.Length > 0)
        {
            float minDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            foreach (Collider col in enemies)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = col.transform;
                }
            }

            enemy = nearestEnemy;

            // Check for attack range
            enemyInSightRange = true;
            enemyInAttackRange = minDistance <= attackRange;

        }
        else
        {
            enemy = null;
            enemyInSightRange = false;
            enemyInAttackRange = false;
        }

        if (agent.velocity.magnitude > 0.1f)
        {
            if (!movementParticleSystem.isPlaying)
            {
                movementParticleSystem.Play();
            }
        }
        else
        {
            if (movementParticleSystem.isPlaying)
            {
                movementParticleSystem.Stop();
            }
        }
        
        if (!enemyInSightRange && !enemyInAttackRange)
            Patroling();

        if (enemyInSightRange && !enemyInAttackRange)
            ChaseEnemy();

        if (enemyInAttackRange && enemyInSightRange)
            AttackEnemy();

    }

    private void Patroling()
    {
        if(!allyRecharge)
        {
            Vector3 newPosition = player.position - player.forward * distanceBehindPlayer;
            agent.SetDestination(newPosition);
        }
    }

    private void ChaseEnemy()
    {
        if(!allyRecharge)
        {
            if (enemy != null && agent.isOnNavMesh)
            {
                agent.SetDestination(enemy.position);
            }
            else
            {
                agent.ResetPath(); 
            }
        }
    }

    private void AttackEnemy()
    {
        if(!allyRecharge)
        {
            agent.SetDestination(transform.position);

            transform.LookAt(enemy);

            if (!alreadyAttacked)
            {
                GameObject proj = Instantiate(projectile, gunTipPosition, Quaternion.identity);
                proj.GetComponent<AllyProjectile>().ally = transform;
                Destroy(proj, 5);
                Rigidbody rb = proj.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 2f, ForceMode.Impulse);

                AudioSource.PlayClipAtPoint(shootingSoundClip, transform.position);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    override
    public void TakeDamage(int damage)
    {
        SetHealth(currentHealth - damage);

        if (currentHealth <= 0) 
        {
            Invoke(nameof(TimeOut), 0.1f);
        }
    }

    private void TimeOut()
    {
        allyRecharge = true;
        gameObject.layer = LayerMask.NameToLayer("Inactive");
            
        StartCoroutine(HealthCharge());                   
    }

    private IEnumerator HealthCharge()
    {
        while(currentHealth < maxHealth)
        {           
            currentHealth += 10;
            healthbar.SetHealth(currentHealth);

            if (!healingParticleSystem.isPlaying)
            {
                healingParticleSystem.Play();
            }

            yield return new WaitForSeconds(2);
        }

        currentHealth = maxHealth;

        if (healingParticleSystem.isPlaying)
        {
            healingParticleSystem.Stop();
        }

        int allyLayer = LayerMask.NameToLayer("Ally");
        gameObject.layer = allyLayer;
        allyRecharge = false; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}