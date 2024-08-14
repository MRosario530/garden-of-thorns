using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour, IEnemyTakeDamage
{
    public BossController bossController;
    public int damage = 0;

    void Start()
    {
        bossController = GameObject.FindGameObjectWithTag("Boss").gameObject.GetComponent<BossController>();
    }

    public void TakeDamage(int damage, Transform attacker = null)
    {
        bossController.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }

        if (other.GetComponent<AllyController>())
        {
            other.GetComponent<AllyController>().TakeDamage(damage);
        }
    }

    public Enemy GetEnemy()
    {
        return bossController;
    }
}
