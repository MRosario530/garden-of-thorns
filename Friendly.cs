using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Friendly : MonoBehaviour, IFriendlyTakeDamage
{
    public int maxHealth;
    public int currentHealth;
    public Healthbar healthbar;

    public abstract void TakeDamage(int damage);

    public void SetHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        currentHealth = newHealth;
        healthbar.SetHealth(newHealth);
    }

    public Friendly GetEnemy()
    {
        return this;
    }
}
