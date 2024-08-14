using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnLerp : MonoBehaviour
{
    public float startingDeltaY;
    private Vector3 targetPos;
    private bool isSpawning;

    void Start()
    {
        targetPos = transform.position;
        transform.position = transform.position + Vector3.up * startingDeltaY;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<EnemyController>().enabled = false;
        isSpawning = true;
    }

    void Update()
    {
        if (isSpawning)
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPosInter = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 3f);
            transform.position = Vector3.MoveTowards(currentPos, targetPosInter, Vector3.Distance(currentPos, targetPosInter));

            if (Vector3.Distance(currentPos, targetPos) < 0.01f)
            {
                isSpawning = false;
                GetComponent<NavMeshAgent>().enabled = true;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<MeshCollider>().enabled = true;
                GetComponent<EnemyController>().enabled = true;
                Destroy(this);
            }
        }
    }
}
