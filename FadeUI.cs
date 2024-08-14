using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour
{

    public CanvasGroup canvas;
    public float fadeSpeed;
    public float desiredAlpha;
    public float startAlpha = 0;
    public float fadeDelay = 0;
    private bool readyToFade = false;

    void Start()
    {
        canvas.alpha = startAlpha;
        canvas.gameObject.SetActive(true);
        Invoke("ReadyToFade", fadeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToFade)
        {
            canvas.alpha = Mathf.MoveTowards(canvas.alpha, desiredAlpha, Time.deltaTime * fadeSpeed);

            if (Mathf.Approximately(canvas.alpha, desiredAlpha))
            {
                Destroy(this);
            }
        }
    }

    void ReadyToFade()
    {
        readyToFade = true;
    }
}
