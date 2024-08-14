using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveDamage : MonoBehaviour
{
    public int damage;
    public float height;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().isDead)
        {
            GetComponent<SphereCollider>().enabled = false;
            return;
        }

        if (other.gameObject.GetComponent<Friendly>() && other.gameObject.transform.position.y < height)
        {
            other.gameObject.GetComponent<Friendly>().TakeDamage(damage);
        }
    }
}
