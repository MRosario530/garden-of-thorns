using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class BossController : Enemy
{
    [SerializeField] private GameStateController gameStateController;
    private NavMeshAgent agent;
    private Animator animator;
    public int slamAttackDamage;
    public int stompAttackDamage;
    public int lowPunchAttackDamage;
    public int oneHandSlamDamage;

    public int groundSlashDamage;
    public int rockSpawnArmsDamage;

    public int rocksToSpawn;
    public bool rocksSpawning;

    public Hitbox[] slamAttackHitboxes;
    public Hitbox[] stompAttackHitboxes;
    public Hitbox[] lowPunchAttackHitboxes;
    public Hitbox[] oneHandSlamHitboxes;
    public Hitbox[] groundSlashHitbox;
    public Hitbox[] rocksToSpawnHitboxes;

    // Currently Active Hitboxes
    private Hitbox[] activeHitboxes = null;

    public bool isAttacking = false;
    public GameObject groundSlashVFX;
    public GameObject slamAttackVFX;
    public GameObject shockwaveVFX;
    public GameObject rockVFX;
    public GameObject rockSummonCircleVFX;

    public float attackRange, chaseRange, tooCloseRange;
    public bool targetInAttackRange, targetInChaseRange, targetInTooCloseRange;
    public bool canAttack;
    public int currentAttackIndex;
    public float timeBetweenAttacks;
    public bool trackPlayer = false;


    void Start()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.SetMaxHealth(maxHealth);
        SetHealth(maxHealth);
        animator = GetComponent<Animator>();
        groundSlashVFX = Resources.Load<GameObject>("Prefabs/VFX/GroundSlash");
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        canAttack = true;
        rocksSpawning = false;
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        targetInAttackRange = distanceToTarget <= attackRange;
        targetInChaseRange = distanceToTarget <= chaseRange;
        targetInTooCloseRange = distanceToTarget <= tooCloseRange;

        if (!isAttacking && (targetInTooCloseRange || !targetInChaseRange))
        {
            Chase();
        }

        if (canAttack && targetInAttackRange && !target.gameObject.GetComponent<PlayerController>().isDead)
        {
            Attack();
        }

        if (trackPlayer)
        {
            transform.LookAt(target);

            switch (currentAttackIndex)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    transform.Rotate(0, 15, 0);
                    break;
                case 5:
                    transform.Rotate(0, 15, 0);
                    break;
            }
        }

        animator.SetFloat("CurrentSpeed", agent.velocity.magnitude);

    }

    void ResetAttackCooldown()
    {
        canAttack = true;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        Invoke(nameof(ResetAttackCooldown), timeBetweenAttacks);
    }

    void Attack()
    {
        canAttack = false;
        trackPlayer = true;
        isAttacking = true;
        agent.ResetPath();
        Invoke(nameof(StopTracking), 1f);

        animator.applyRootMotion = true;
        if (rocksSpawning)
        {
            currentAttackIndex = UnityEngine.Random.Range(1, 6);
        }
        else
        {
            currentAttackIndex = UnityEngine.Random.Range(1, 7);
        }

        switch (currentAttackIndex)
        {
            case 1:
                animator.SetTrigger("SlamAttack");
                break;
            case 2:
                animator.SetTrigger("StompAttack");
                break;
            case 3:
                animator.SetTrigger("LowPunchAttack");
                break;
            case 4:
                animator.SetTrigger("1HandSlamAttack");
                break;
            case 5:
                animator.SetTrigger("GroundSlashAttack");
                break;
            case 6:
                animator.SetTrigger("RockSpawnAttack");
                break;
        }
    }

    private void Chase()
    {
        Vector3 newPosition = target.position + (transform.position - target.position).normalized * chaseRange;
        agent.SetDestination(newPosition);
    }

    void StopTracking()
    {
        trackPlayer = false;
    }

    void ApplyDamage(int damage)
    {
        foreach (Hitbox hitbox in activeHitboxes)
        {
            hitbox.damage = damage;
        }
    }

    void RemoveDamage()
    {
        foreach (Hitbox hitbox in activeHitboxes)
        {
            hitbox.damage = 0;
        }
        activeHitboxes = null;
        animator.applyRootMotion = false;
    }

    void EnableAttackDamage(String attackName)
    {
        switch (attackName)
        {
            case "SlamAttack":
                activeHitboxes = slamAttackHitboxes;
                ApplyDamage(slamAttackDamage);
                break;
            case "StompAttack":
                activeHitboxes = stompAttackHitboxes;
                ApplyDamage(stompAttackDamage);
                break;
            case "LowPunchAttack":
                activeHitboxes = lowPunchAttackHitboxes;
                ApplyDamage(lowPunchAttackDamage);
                break;
            case "1HandSlamAttack":
                activeHitboxes = oneHandSlamHitboxes;
                ApplyDamage(oneHandSlamDamage);
                break;
            case "GroundSlashAttack":
                activeHitboxes = groundSlashHitbox;
                ApplyDamage(groundSlashDamage);
                GroundSlashAttack();
                break;
            case "RockSpawnAttack":
                activeHitboxes = rocksToSpawnHitboxes;
                ApplyDamage(rockSpawnArmsDamage);
                break;
        }
    }

    void GroundSlashAttack()
    {
        var projectile = Instantiate(groundSlashVFX, new Vector3(activeHitboxes[0].gameObject.transform.position.x, 0 , activeHitboxes[0].gameObject.transform.position.z), Quaternion.identity) as GameObject;
        GroundSlash groundSlashScript = projectile.GetComponent<GroundSlash>();
        groundSlashScript.damage = groundSlashDamage;
        RotateToDestination(projectile, transform.forward * 1000, false);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;
    }

    void SlamAttackCone()
    {
        GameObject temp = Instantiate(slamAttackVFX, new Vector3(activeHitboxes[0].gameObject.transform.position.x, 0, activeHitboxes[0].gameObject.transform.position.z), Quaternion.identity);
        var rotation = Quaternion.LookRotation(transform.forward);
        temp.transform.localRotation = rotation;
        Destroy(temp, 2.2f);
    }

    void Shockwave()
    {
        GameObject temp = Instantiate(shockwaveVFX, new Vector3(activeHitboxes[0].gameObject.transform.position.x, 0, activeHitboxes[0].gameObject.transform.position.z), Quaternion.identity);
        Destroy(temp, 1f);
    }

    void SpawnRocksAttack()
    {
        Instantiate(rockSummonCircleVFX, new Vector3(transform.position.x, 0, transform.position.z), rockSummonCircleVFX.transform.rotation);
        rocksSpawning = true;
        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        for (int i = 0; i < rocksToSpawn; i++)
        {
            Instantiate(rockVFX, new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z), Quaternion.identity);
            if (target.GetComponent<PlayerController>().isDead)
                break;
            yield return new WaitForSeconds(2f);
        }
        rocksSpawning = false;
    }

    void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);

        if (onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;
        }

        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    public override void OnDeath()
    {
        animator.SetInteger("deathCondition", UnityEngine.Random.Range(1, 5));
        healthBar.gameObject.SetActive(false);
        gameStateController.VictoryScreenSequence();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);
    }

}
