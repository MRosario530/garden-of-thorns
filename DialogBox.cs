using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI continueText;
    public string message;

    [SerializeField] private AudioClip openTextSoundClip;
    [SerializeField] private AudioClip closeTextSoundClip;
    [SerializeField] private AudioSource audioSRC;

    public void SetText(string text)
    {
        message = text;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSRC.PlayOneShot(closeTextSoundClip);
            Destroy(gameObject, 0.2f);
        }

        continueText.alpha = Mathf.PingPong(Time.time * 0.5f, 1);
    }

    public void StartDisplay()
    {
        audioSRC.PlayOneShot(openTextSoundClip);
        StartCoroutine(DisplayMessage(message));
    }

    IEnumerator DisplayMessage(string message)
    {
        textMeshPro.text = string.Empty;

        for (int i = 0; i < message.Length; i++)
        {
            textMeshPro.text += message[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

}
