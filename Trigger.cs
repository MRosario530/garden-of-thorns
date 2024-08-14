using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    [SerializeField] string tagFilter;
    [SerializeField] bool destroyOnEnter;
    [SerializeField] bool destroyOnExit;
    [SerializeField] int delay = 0;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        // Additional initialization code can be placed here
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DelayedTriggerEnter(other));
    }

    void OnTriggerExit(Collider other)
    {
        StartCoroutine(DelayedTriggerExit(other));
    }

    IEnumerator DelayedTriggerEnter(Collider other)
    {
        if (!String.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
            yield break;

        yield return new WaitForSeconds(delay);

        onTriggerEnter.Invoke();

        if (destroyOnEnter)
            Destroy(gameObject);
    }

    IEnumerator DelayedTriggerExit(Collider other)
    {
        if (!String.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
            yield break;

        yield return new WaitForSeconds(delay);

        onTriggerExit.Invoke();

        if (destroyOnExit)
            Destroy(gameObject);
    }
}
