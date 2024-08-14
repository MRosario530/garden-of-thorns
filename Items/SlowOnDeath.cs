using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    [SerializeField] private Enemy enemy;
    [SerializeField] private bool hasFroze = false;
    
    // Start is called before the first frame update
    void Start()
    {
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Freeze circle");
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.currentHealth <= 0 && !hasFroze)
        {
            Instantiate(vfx, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            hasFroze = true;
        }
    }
}
