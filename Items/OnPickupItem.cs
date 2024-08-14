using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnPickupItem : Item
{
    public abstract void OnPickup(PlayerController playerController);

    public void ApplyEffect()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            OnPickup(playerController);
        }
    }
}
