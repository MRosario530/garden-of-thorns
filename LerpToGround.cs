using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngineInternal;

public class LerpToGround : MonoBehaviour
{
    private Vector3 targetPos;

    void Start()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<EnemyController>().enabled = false;

        // Make sure raycast can hit the terrain from below
        Physics.queriesHitBackfaces = true;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            targetPos = hit.point;
        }
        else if (Physics.Raycast(transform.position, Vector3.up, out hit, 100))
        {
            targetPos = hit.point;
        }
        else
        {
            targetPos = transform.position;
        }

        Physics.queriesHitBackfaces = false;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPosInter = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 3f);
        transform.position = Vector3.MoveTowards(currentPos, targetPosInter, Vector3.Distance(currentPos, targetPosInter));

        if (Vector3.Distance(currentPos, targetPos) < 0.01f)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<MeshCollider>().enabled = true;
            GetComponent<EnemyController>().enabled = true;
            Destroy(this);
        }
    }
}
