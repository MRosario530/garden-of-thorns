using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private float bulletSpeed = 70f;
    [SerializeField] public int bulletDamage = 10;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] public GameObject itemHolder;
    public Transform player;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IEnemyTakeDamage>() != null)
        {
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);

            IEnemyTakeDamage enemyTakeDamage = other.GetComponent<IEnemyTakeDamage>();
            enemyTakeDamage.TakeDamage(bulletDamage, player);

            Enemy enemy = enemyTakeDamage.GetEnemy();
            ApplyEffects(enemy);
        }
        else
        {
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void ApplyEffects(Enemy enemy)
    {
        foreach (OnHitItem item in itemHolder.GetComponentsInChildren<OnHitItem>())
        {
            item.OnHit(enemy);
            if (enemy == null)
                return;
        }
    }
}
