using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFriendlyTakeDamage
{
    void TakeDamage(int damage);

    Friendly GetEnemy();
}
