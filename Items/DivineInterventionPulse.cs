using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX.Utility;

public class DivineInterventionPulse : MonoBehaviour
{
    public float effectTime = 10f;
    public GameObject vfx;

    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Instantiate(vfx, other.transform);
            other.AddComponent<DivineInterventionEffect>();
        }
    }
}
