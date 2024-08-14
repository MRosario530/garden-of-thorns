using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    public Transform ally;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IEnemyTakeDamage>() != null)
        {
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
            IEnemyTakeDamage enemyTakeDamage = other.GetComponent<IEnemyTakeDamage>();
            enemyTakeDamage.TakeDamage(damage, ally);
        }
        else
        {
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}