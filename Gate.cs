using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gate : MonoBehaviour
{
    public float openY;
    public float closeY;

    private Vector3 targetPos;
    private bool isMoving;

    void Start()
    {
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPosInter = Vector3.Lerp(currentPos, targetPos, Time.deltaTime);
            transform.position = Vector3.MoveTowards(currentPos, targetPosInter, Vector3.Distance(currentPos, targetPosInter));

            if (Vector3.Distance(currentPos, targetPos) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void Open()
    {
        targetPos = new Vector3(transform.position.x, openY, transform.position.z);
        isMoving = true;
    }

    public void Close()
    {
        targetPos = new Vector3(transform.position.x, closeY, transform.position.z);
        isMoving = true;
    }
}
