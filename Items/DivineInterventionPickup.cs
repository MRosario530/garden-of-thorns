using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DivineInterventionPickup : OnPickupItem
{
    override
    public void OnPickup(PlayerController playerController)
    {
        playerController.AddComponent<DivineIntervention>();
    }
}
