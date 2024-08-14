using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSwordItem : OnHitItem
{
    private GameObject vfx;
    private GameObject player;
    private int chanceToProc = 25;


    private void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Homing Sword");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    override
    public void OnHit(Enemy enemy)
    {
        System.Random random = new();
        int rand = random.Next(100);
        if (rand > chanceToProc)
            return;
        GameObject temp = Instantiate(vfx, player.transform.position + new Vector3(1, 1, 0), vfx.transform.rotation);
        temp.GetComponent<HomingAttack>().target = enemy.gameObject;
    }

}
