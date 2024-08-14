using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : OnHitItem
{
    private int explosionDamage = 45;
    private int chanceToProc = 15;
    private int radius = 9;

    private GameObject vfx;

    public void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Explosion");
    }

    override
    public void OnHit(Enemy enemy)
    {
        System.Random random = new System.Random();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;

        GameObject temp = Instantiate(vfx, enemy.transform.position + Vector3.up, Quaternion.identity);
        if (enemy is BossController)
        {
            temp.transform.position = enemy.transform.position + new Vector3(0, 5, 0);
        }
        temp.GetComponent<ParticleSystem>().Play(true);
        temp.GetComponent<AudioSource>().Play();

        enemy.TakeDamage(explosionDamage);
        DetermineInExplosion();
        Destroy(temp, 3);
    }

    private void DetermineInExplosion()
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, radius);

        foreach (var surroundingObject in surroundingObjects)
        {
            if (surroundingObject.GetComponent<Enemy>())
            {
                Enemy enemy = surroundingObject.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(explosionDamage);
            }
        }
    }
}
