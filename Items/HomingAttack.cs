using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttack : MonoBehaviour
{

    public GameObject target;
    public float speed = 0f;
    private Vector3 targetLastPosition;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 hitVFXSize;
    // Start is called before the first frame update
    void Start()
    {
        if (target != null && target.GetComponent<BossController>())
        {
            offset = new Vector3(0f, 6f, 0f);
            hitVFXSize = new Vector3(3f, 3f, 3f);
        }
        else
        {
            offset = new Vector3(0f, 0.6f, 0f);
            hitVFXSize = Vector3.one;
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
            transform.position += speed * Time.deltaTime * (targetLastPosition  - transform.position).normalized;
            speed += 0.05f;
            transform.LookAt(targetLastPosition);
            if (target != null)
                targetLastPosition = target.transform.position + offset;
            yield return null;
        }

        if (target != null && target.GetComponent<Enemy>())
        {
            target.GetComponent<Enemy>().TakeDamage(35);
        }

        Instantiate(hitVFX, transform.position, Quaternion.identity).transform.localScale = hitVFXSize;

        Destroy(gameObject);
    }
}
