using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningBullet : OnHitItem
{
    private int lightningDamage = 80;
    private int chanceToProc = 15;
    [SerializeField] private GameObject vfx;

    public void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Lightning Strike");
    }

    override
    public void OnHit(Enemy enemy)
    {
        System.Random random = new System.Random();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;

        GameObject temp = Instantiate(vfx, enemy.transform.position, Quaternion.identity);
        if (enemy is BossController)
        {
            temp.transform.localScale = new Vector3(3, 3, 3);
        }
        temp.GetComponent<ParticleSystem>().Play(true);
        temp.GetComponent<AudioSource>().Play();
        enemy.TakeDamage(lightningDamage);
        Destroy(temp, 2);
    }
}
