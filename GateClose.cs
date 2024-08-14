using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateClose : MonoBehaviour
{
    public Transform target;
    public float speed;

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = Vector3.Lerp(currentPos, target.position, Time.deltaTime * speed);
        
        // Move towards the calculated position
        transform.position = Vector3.MoveTowards(currentPos, targetPos, Vector3.Distance(currentPos, targetPos));
    }
}
