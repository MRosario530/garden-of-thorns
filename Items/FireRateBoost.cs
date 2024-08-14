using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBoost : OnPickupItem
{
    [SerializeField] private float fireRateMultiplier = 1.5f;

    override
    public void OnPickup(PlayerController playerController)
    {
        ThirdPersonShooterController thirdPersonController = playerController.GetComponent<ThirdPersonShooterController>();

        thirdPersonController.fireRate *= fireRateMultiplier;
    }
}
