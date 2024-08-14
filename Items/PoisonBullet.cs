using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonBullet : OnHitItem
{
    override
    public void OnHit(Enemy enemy)
    {
        if (!enemy.GetComponent<PoisonDamage>())
        {
            enemy.AddComponent<PoisonDamage>();
        }
        else
        {
            enemy.GetComponent<PoisonDamage>().timeOfEffect = 3;
        }
    }
}
