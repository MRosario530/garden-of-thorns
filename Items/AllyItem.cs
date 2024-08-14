using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyItem : OnPickupItem
{
    [SerializeField] private GameObject allyPrefab;

    override
    public void OnPickup(PlayerController playerController)
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Instantiate(allyPrefab, playerTransform.position + playerTransform.forward, Quaternion.identity);
    }
}
