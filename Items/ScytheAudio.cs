using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheAudio : MonoBehaviour
{
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(PlayScytheAudio), 5.8f);
    }

    private void PlayScytheAudio()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
