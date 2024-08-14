using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealItem : OnHitItem
{
    PlayerController controller;
    private int healingAmount = 7;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindAnyObjectByType(typeof(PlayerController)) as PlayerController;
    }

    override
    public void OnHit(Enemy enemy)
    {
        if (controller.maxHealth != controller.currentHealth)
        {
            controller.SetHealth(controller.currentHealth + healingAmount);
        }
    }
}
