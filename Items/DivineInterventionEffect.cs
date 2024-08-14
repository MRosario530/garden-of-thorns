using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DivineInterventionEffect : MonoBehaviour
{
    public float effectTimer = 10f;
    private float originalSpeed = 0f;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        effectTimer -= Time.deltaTime;
        if (effectTimer <= 0)
        {
            agent.speed = originalSpeed;
            Destroy(this);
        }
        else if (agent.speed >= 0)
        {
            agent.speed = 0;
        }
    }
}
