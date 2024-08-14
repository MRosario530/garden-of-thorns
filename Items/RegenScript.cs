using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenScript : MonoBehaviour
{
    PlayerController playerController;
    float timeBetweenHeals = 5f;
    int healingAmount = 5;
    bool canHeal;

    // Start is called before the first frame update
    void Start()
    {
        canHeal = true;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canHeal)
        {
            canHeal = false;
            Invoke(nameof(ResetHealCooldown), timeBetweenHeals);

            if (playerController != null && playerController.currentHealth < playerController.maxHealth)
            {
                playerController.SetHealth(playerController.currentHealth + healingAmount);
            }
        }
    }

    void ResetHealCooldown()
    {
        canHeal = true;
    }
}
