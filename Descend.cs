using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descend : MonoBehaviour
{
    Vector3 destination;

    void Start()
    {
        // raycast to find the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // set the position of the object to the hit point
            destination = hit.point;
        }
    }

    void Update()
    {
        // move the object towards the destination
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime);
    }
}
