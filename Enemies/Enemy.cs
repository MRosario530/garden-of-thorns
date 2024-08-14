using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemyTakeDamage
{
    public int maxHealth;
    public int currentHealth;
    public Healthbar healthBar;
    public RoomController roomController;
    public Transform target = null;
    public bool isAlive = true;

    public virtual void TakeDamage(int damage, Transform attacker = null)
    {
        if (!isAlive)
        {
            return;
        }

        SetHealth(currentHealth - damage);

        if (target == null && attacker != null)
        {
            target = attacker;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            Invoke(nameof(OnDeath), 0.1f);
        }
    }

    public void SetHealth(int newHealth)
    {
        currentHealth = newHealth;
        healthBar.SetHealth(newHealth);
    }

    public abstract void OnDeath();

    public Enemy GetEnemy()
    {
        return this;
    }
}
