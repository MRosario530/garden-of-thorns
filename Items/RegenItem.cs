using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting; // Add this line


public class RegenItem : OnPickupItem
{
    // Start is called before the first frame update
    public float timerReduction;
    
    override
    public void OnPickup(PlayerController playerController)
    {
        playerController.timeBetweenHeals -= timerReduction;
    }
}
