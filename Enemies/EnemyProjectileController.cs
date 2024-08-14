using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Transform vfxHitGreen;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IFriendlyTakeDamage>() != null)
        {
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);

            IFriendlyTakeDamage friendlyTakeDamage = other.GetComponent<IFriendlyTakeDamage>();
            friendlyTakeDamage.TakeDamage(damage);
        }
        else
        {
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}