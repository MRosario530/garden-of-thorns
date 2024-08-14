using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : OnPickupItem
{

    override
    public void OnPickup(PlayerController playerController)
    {
        playerController.GetComponent<ThirdPersonController>().JumpHeight *= 2;
    }
}
