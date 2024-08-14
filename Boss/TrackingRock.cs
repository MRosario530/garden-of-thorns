using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingRock : MonoBehaviour
{
    private SphereCollider hitbox;
    private AudioSource rockAudio;

    public float colliderStartTime;
    public float audioStartTime;
    public int rockDamage;

    // Start is called before the first frame update
    void Start()
    {
        rockAudio = GetComponent<AudioSource>();
        hitbox = GetComponent<SphereCollider>();
        Invoke(nameof(PlayRockAudio), audioStartTime);
        Invoke(nameof(SetHitboxActive), colliderStartTime);
    }

    void PlayRockAudio()
    {
        rockAudio.Play();
    }

    void SetHitboxActive()
    {
        hitbox.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Friendly>())
        {
            if (other.GetComponentInChildren<ShieldEffect>() != null && other.GetComponentInChildren<ShieldEffect>().shieldActive)
            {
                other.GetComponentInChildren<ShieldEffect>().TakeDamage(rockDamage);
                hitbox.enabled = false;
            }
            else
            {
                other.GetComponent<Friendly>().TakeDamage(rockDamage);
            }
        }
    }

}
