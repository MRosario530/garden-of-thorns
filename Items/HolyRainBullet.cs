using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyRainBullet : OnHitItem
{
    private int chanceToProc = 7;
    [SerializeField] private GameObject vfx;

    public void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Laser Rain");
    }

    override
    public void OnHit(Enemy enemy)
    {
        System.Random random = new System.Random();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;

        GameObject temp = Instantiate(vfx, new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z), Quaternion.identity);
        temp.GetComponent<ParticleSystem>().Play(true);
        Destroy(temp, 5);
    }
}
