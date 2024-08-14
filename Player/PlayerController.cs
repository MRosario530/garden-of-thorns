using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : Friendly
{
    public GameStateController gs;
    public bool isDead;

    public float timeBetweenHeals;
    public int healingAmount;
    bool canHeal;

    [SerializeField] private InventoryController inventoryController;

    void Start()
    {
        isDead = false;
        canHeal = true;
        healthbar.SetMaxHealth(maxHealth);
        SetHealth(maxHealth);
    }

    void Update()
    {
        if (canHeal)
        {
            canHeal = false;
            Invoke(nameof(ResetHealCooldown), timeBetweenHeals);
            SetHealth(currentHealth + healingAmount);
        }
    }

    override
    public void TakeDamage(int damage)
    {
        SetHealth(currentHealth - damage);
        if (currentHealth <= 0)
        {
            isDead = true;
            gameObject.layer = LayerMask.NameToLayer("Inactive");
            gs.OnDeath();
        }
    }

    public void AddItemToInventory(Item item)
    {
        inventoryController.AddItemToInventory(item.name, item.sprite);
    }
    void ResetHealCooldown()
    {
        canHeal = true;
    }
}
