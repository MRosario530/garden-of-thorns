using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingLightning : MonoBehaviour
{

    public GameObject target;
    public float speed = 20f;
    private Vector3 targetLastPosition;
    [SerializeField] private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        if (target != null && target.GetComponent<BossController>())
        {
            offset = new Vector3(0f, 6f, 0f);
        }
        else
        {
            offset = new Vector3(0f, 0.6f, 0f);
        }
        StartCoroutine(Homing());
    }

    public IEnumerator Homing()
    {
        if (target == null)
            yield break;

        targetLastPosition = target.transform.position + offset;

        while (Vector3.Distance(targetLastPosition, transform.position) > 0.6f)
        {
            transform.position += speed * Time.deltaTime * (targetLastPosition - transform.position).normalized;
            speed += 0.05f;
            transform.LookAt(targetLastPosition);
            if (target != null)
                targetLastPosition = target.transform.position + offset;
            yield return null;
        }

        Destroy(gameObject);
    }
}
