using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParticleDamage : MonoBehaviour
{
    public int damageValue;
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Friendly>())
            other.GetComponent<Friendly>().TakeDamage(damageValue);
    }
}
