using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackItem : OnHitItem
{
    private int force = 20;
    private int damage = 8;
    private int chanceToProc = 40;


    override
    public void OnHit(Enemy enemy)
   {
        System.Random random = new();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;

        if (enemy.GetComponent<Rigidbody>())
        {
            enemy.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        }
        enemy.TakeDamage(damage);
   }
}
