using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathMarkItem : OnHitItem
{
    [SerializeField] private GameObject vfx;
    private GameObject slice;
    private int chanceToProc = 20;
    private bool marked = false;

    private void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Death Slice");
    }


    override
    public void OnHit(Enemy enemy)
    {
        if (marked)
            return;
        System.Random random = new();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;
        
        slice = Instantiate(vfx, enemy.transform.position, Quaternion.identity);
        slice.transform.parent = enemy.transform;
        if (enemy is BossController)
        {
            slice.transform.localScale = new Vector3(3,3,3);
        }
        slice.GetComponent<ParticleSystem>().Play();
        marked = true;
        StartCoroutine(DelayedDamage(enemy));
    }

    private IEnumerator DelayedDamage(Enemy enemy)
    {
        yield return new WaitForSeconds(5.85f);
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().TakeDamage(55);
        }
        yield return new WaitForSeconds(1.5f);
        if (slice != null)
        {
            slice.GetComponent<ParticleSystem>().Stop();
        }
        marked = false;
    }

}
