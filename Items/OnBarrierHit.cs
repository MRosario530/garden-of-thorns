using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBarrierHit : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other)
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        if (other.GetComponent<EnemyProjectileController>())
        {
            Destroy(other);
        }
    }
}
