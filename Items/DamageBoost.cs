using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets; // Add this line

public class DamageBoost : OnHitItem
{
    [SerializeField] private int damageBoost = 10;

    override
    public void OnHit(Enemy enemy)
    {
        enemy.TakeDamage(damageBoost);
    }

}