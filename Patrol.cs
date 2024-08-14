using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public NavMeshAgent agent;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float patrolSpeed;
    public float timeBetweenPatrols;
    public bool canPatrol;

    void Awake()
    {
        agent.speed = patrolSpeed;
        walkPointSet = false;
        canPatrol = true;
    }

    void Update()
    {
        DoPatrol();
    }

    void DoPatrol()
    {
        agent.speed = patrolSpeed;

        if (!walkPointSet && canPatrol)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (walkPointSet)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                walkPointSet = false;
                canPatrol = false;
                Invoke(nameof(ResetPatrol), timeBetweenPatrols);
            }
        }
    }

    void ResetPatrol()
    {
        canPatrol = true;
    }

    void SearchWalkPoint()
    {
        float range = 5f;
        if (RandomPoint(transform.position, range, out walkPoint))
        {
            walkPointSet = true;
            Debug.DrawRay(walkPoint, Vector3.up, Color.blue, 1.0f);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomPointInUnitCircle2D = Random.insideUnitCircle;
            Vector3 randomPointInUnitCircle3D = new Vector3(randomPointInUnitCircle2D.x, 0, randomPointInUnitCircle2D.y);
            Vector3 randomPoint = center + randomPointInUnitCircle3D * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
