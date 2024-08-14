using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShieldEffect : Friendly
{
    private ParticleSystem _particleSystem;
    private SphereCollider _sphereCollider;
    private PlayerController _playerController;
    private GameStateController _gameStateController;
    public bool shieldActive;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GameObject.Find("GameStateManager/UI Canvas/Shield Health Bar").GetComponent<Healthbar>();
        healthbar.gameObject.SetActive(true);
        _particleSystem = GetComponent<ParticleSystem>();
        _sphereCollider = GetComponent<SphereCollider>();
        _playerController = GetComponentInParent<PlayerController>();
        _gameStateController = _playerController.gs;
        shieldActive = true;
    }

    private void Update()
    {
        if (_playerController.isDead || _gameStateController.inEndSequence)
        {
            Destroy(gameObject);
        }
    }

    override
    public void TakeDamage(int damage)
    {
        int damageRemaining = currentHealth - damage;
        SetHealth(damageRemaining);
        if (damageRemaining < 0)
        {
            _playerController.TakeDamage(-damageRemaining);
        }

        if (currentHealth <= 0)
        {
            SetShieldOnCooldown();
        }
    }

    void SetShieldOnCooldown()
    {
        _particleSystem.Stop(true);
        _particleSystem.Clear(true);
        _sphereCollider.enabled = false;
        shieldActive = false;
        StartCoroutine(UpdateHealthBar());
    }

    IEnumerator UpdateHealthBar()
    {
        while (currentHealth < maxHealth)
        {
            yield return new WaitForSeconds(2.5f);
            SetHealth(currentHealth + 10);
        }
        _particleSystem.Play(true);
        yield return new WaitForSeconds(1f);
        _sphereCollider.enabled = true;
        shieldActive = true;
    }
}
