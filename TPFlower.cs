using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPFlower : MonoBehaviour
{
    public GameObject flower;
    private float initializationTime;
    private float scaleSpeed = 2F;

    // Start is called before the first frame update
    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;   // Determines the starting time to ensure the plant cannot grow on the first frame.
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 v3Velocity = rb.velocity;

        if (v3Velocity == new Vector3(0,0,0) && timeSinceInitialization > 0.1)  // If the block has stopped moving, grow a flower from it.
        {
            flower.SetActive(true);
            if (flower.transform.localScale[0] < 15)
                flower.transform.localScale += (new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime);
        }
        else
        {
            flower.SetActive(false);
        }
    }
}
