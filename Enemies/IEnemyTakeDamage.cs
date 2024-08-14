using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyTakeDamage
{
    void TakeDamage(int damage, Transform attacker = null);

    Enemy GetEnemy();
}
