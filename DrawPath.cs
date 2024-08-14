using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawPath : MonoBehaviour
{
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 currentPoint = transform.position;
        if (agent != null && agent.path != null)
        {
            foreach (Vector3 point in agent.path.corners)
            {
                Gizmos.DrawLine(currentPoint, point);
                currentPoint = point;
            }
        }
    }
}
