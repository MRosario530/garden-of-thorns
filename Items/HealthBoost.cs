using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets; // Add this line

public class HealthBoost: OnPickupItem
{
    [SerializeField] private int healthBoost = 50;
    [SerializeField] private GameObject healingVFX;

    override
    public void OnPickup(PlayerController playerController)
    {
        playerController.maxHealth += healthBoost;
        playerController.healthbar.SetMaxHealth(playerController.maxHealth);
        playerController.SetHealth(playerController.maxHealth);
        Instantiate(healingVFX, playerController.transform.position, Quaternion.identity) ;
    }
}
