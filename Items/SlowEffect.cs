using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowEffect : MonoBehaviour
{
    public ParticleSystem part;
    public float removedSpeed;
    public float effectTime;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(5);
            StartCoroutine(SlowEffectTimer(other.gameObject.GetComponent<NavMeshAgent>()));
        }
    }

    IEnumerator SlowEffectTimer(NavMeshAgent enemyNav)
    {
        enemyNav.speed -= removedSpeed;
        yield return new WaitForSeconds(effectTime);
        if (enemyNav != null)
            enemyNav.speed += removedSpeed;
    }
}
