using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [Header("Oscillation and Rotation")]
    public float oscillationSpeed = 1f;
    public float oscillationHeight = 1f;
    public float rotationSpeed = 50f;

    void Update()
    {
        Vector3 pos = transform.localPosition;

        // Vertical oscillation
        float newY = (Mathf.Sin(Time.time * oscillationSpeed) + 1) / 2;
        transform.localPosition = new Vector3(pos.x, newY * oscillationHeight, pos.z);

        // Rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
