using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets; // Add this line

public class SpeedBoost : OnPickupItem
{
    [SerializeField] private float speedMultiplier = 1.2f;

    override
    public void OnPickup(PlayerController playerController)
    {
        ThirdPersonController thirdPersonController = playerController.GetComponent<ThirdPersonController>();

        thirdPersonController.MoveSpeed *= speedMultiplier;
        thirdPersonController.SprintSpeed *= speedMultiplier;
    }
}