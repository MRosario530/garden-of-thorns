using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets; // Add this line


public class ShieldItem : OnPickupItem
{
    private GameObject shieldVFX;
    private void Start()
    {
        shieldVFX = Resources.Load<GameObject>("Prefabs/VFX/Magic shield");
    }

    override
    public void OnPickup(PlayerController playerController)
    {
        Instantiate(shieldVFX, playerController.transform.position, Quaternion.identity).transform.parent = playerController.transform;
    }
}
