using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingSparkItem : OnHitItem
{
    private int chanceToProc = 35;
    override
    public void OnHit(Enemy enemy)
    {
        System.Random random = new System.Random();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;

        if (!enemy.GetComponent<SparkEffect>())
        {
            enemy.AddComponent<SparkEffect>();
        }
    }
}

