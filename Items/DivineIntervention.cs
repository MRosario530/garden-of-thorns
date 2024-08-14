using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineIntervention : MonoBehaviour
{
    public float cooldown = 90f;
    private float currentCD = 0f;
    private PlayerController playerController;
    private GameObject vfx;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        vfx = Resources.Load<GameObject>("Prefabs/VFX/Divine Intervention");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCD >= 0f)
            currentCD -= Time.deltaTime;
        if (playerController.currentHealth <= 20 && currentCD <= 0f)
        {
            playerController.SetHealth(playerController.maxHealth);
            Instantiate(vfx, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            currentCD = cooldown;
        }
    }
}
