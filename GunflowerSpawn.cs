using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunflowerSpawn : MonoBehaviour
{
   public RoomManager roomManager;

   void Start()
    {
        foreach(EnemyController enemy in roomManager.enemies)
        {
            GameObject gf = enemy.gameObject;    

            GateClose gc = gf.GetComponent<GateClose>();
            if(gc != null)
                gc.enabled = false;

            UnityEngine.AI.NavMeshAgent nma = gf.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if(nma != null)
                nma.enabled = true;

            Rigidbody rb = gf.GetComponent<Rigidbody>();
            if(rb != null)
                rb.useGravity = true;

            MeshCollider mc = gf.GetComponent<MeshCollider>();
            if(mc != null)
                mc.enabled = true;

            EnemyController ec = gf.GetComponent<EnemyController>();
            if(ec != null)
                ec.enabled = true;
        }
    }
}
