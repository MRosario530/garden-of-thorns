using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstHitMeteorItem : OnHitItem
{
    [SerializeField] private GameObject vfx;
    private GameObject meteor;
    [SerializeField] private float cooldown;
    [SerializeField] private float currentCD;

    private void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Meteor 2");
        currentCD = 0f;
        cooldown = 5f;
    }

    private void Update()
    {
        if (currentCD > 0f)
        {
            currentCD -= Time.deltaTime;
        }
    }

    override
    public void OnHit(Enemy enemy)
    {
        if (enemy.currentHealth >= (enemy.maxHealth * 0.6) && currentCD <= 0)
        {
            meteor = Instantiate(vfx, new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z), Quaternion.identity);
            currentCD = cooldown;
            Invoke(nameof(DestroyMeteor), 3f);
            return;
        }
    }

    private void DestroyMeteor()
    {
        if (meteor != null)
        {
            Destroy(meteor);
        }
    }

}
