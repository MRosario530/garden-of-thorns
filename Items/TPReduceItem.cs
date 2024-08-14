using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPReduceItem : OnPickupItem
{

    override
    public void OnPickup(PlayerController playerController)
    {
        playerController.GetComponent<TPAbility>()._teleportCooldown = 3f;
    }
}
