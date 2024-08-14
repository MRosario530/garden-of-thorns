using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SparkEffect : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    [SerializeField] private GameObject jumpingLightningVFX;

    // Start is called before the first frame update
    void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Lightning aura");
        jumpingLightningVFX = Resources.Load<GameObject>("Prefabs/VFX/Jumping Electricity");
        GameObject temp = Instantiate(vfx, transform.position, Quaternion.identity);
        if (gameObject.GetComponent<BossController>())
        {
            temp.transform.localScale = new Vector3(6, 6, 6);
        }
        temp.transform.parent = transform;
        Invoke(nameof(JumpToNext), 3f);
    }

    public void JumpToNext()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 9f);

        foreach (var other in enemies)
        {
            if (other.GetComponent<Enemy>() && !other.GetComponent<SparkEffect>())
            {
                other.AddComponent<SparkEffect>();
                other.GetComponent<Enemy>().TakeDamage(25);
                GameObject lightningStrike = Instantiate(jumpingLightningVFX, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                lightningStrike.GetComponent<JumpingLightning>().target = other.gameObject;
            }
        }
        Destroy(this, 5f);
    }
}
