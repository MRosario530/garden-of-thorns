using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowEffectBullet : OnHitItem
{
    override
    public void OnHit(Enemy enemy)
    {
        if (!enemy.GetComponent<SlowOnDeath>())
            enemy.AddComponent<SlowOnDeath>();
    }
}
