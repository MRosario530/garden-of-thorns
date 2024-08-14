using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX.Utility;

public class EnemyDisableEffect : MonoBehaviour
{
    public ParticleSystem part;
    public float effectTime = 5.0f;
    public GameObject vfx;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("here");
        if (other.GetComponent<Enemy>())
        {
            StartCoroutine(SlowEffectTimer(other.gameObject));
        }
    }

    IEnumerator SlowEffectTimer(GameObject other)
    {
        other.GetComponent<Enemy>().enabled = false;
        yield return new WaitForSeconds(effectTime);
        if (other != null)
            other.GetComponent<Enemy>().enabled = true;
    }
}
