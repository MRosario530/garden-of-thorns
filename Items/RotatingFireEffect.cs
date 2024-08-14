using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFireEffect : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private GameObject vfx;
    private GameStateController gameStateController;


    public void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameStateController = playerController.gs;
    }

    private void Update()
    {
        gameObject.transform.rotation = Quaternion.identity;
        if (playerController.isDead || gameStateController.inEndSequence)
            Destroy(transform.parent.gameObject);
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(25);
            Instantiate(vfx, other.transform.position, Quaternion.identity);
        }
    }
}
