using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingFireItem : OnPickupItem
{
    [SerializeField] private GameObject fireVFX;

    override
    public void OnPickup(PlayerController playerController)
    {
        Instantiate(fireVFX, playerController.transform.position + new Vector3(0, 1, 0), Quaternion.identity).transform.parent = playerController.transform;
    }
}
