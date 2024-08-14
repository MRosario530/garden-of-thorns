using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public List<EnemyController> enemies = new List<EnemyController>(); // Use List for dynamic size
    private int defeatedEnemiesCount;
    [SerializeField] UnityEvent enemiesWiped;
    [SerializeField] UnityEvent afterDelay;
    [SerializeField] bool destroyAfter;
    [SerializeField] int delay = 0;

    void Start()
    {
        defeatedEnemiesCount = 0;

        // Find all components of type Enemy within the current GameObject's hierarchy
        // Only consider enemies that are children of this RoomManager
        EnemyController[] foundEnemies = GetComponentsInChildren<EnemyController>(true);
        
        foreach (EnemyController enemy in foundEnemies)
        {
            if (enemy.transform.IsChildOf(transform))
            {
                enemies.Add(enemy);
            }
        }
    }

    // Coroutine method to wait for a specified duration and then invoke the event
    IEnumerator WaitAndInvoke()
    {
        
        enemiesWiped.Invoke();

        yield return new WaitForSeconds(delay);

        afterDelay.Invoke();

        if (destroyAfter)
            Destroy(gameObject);
    }

    // Call this method when an enemy is defeated
    public void EnemyDefeated()
    {
        defeatedEnemiesCount++;
        Debug.Log("Enemy Defeated, total " + defeatedEnemiesCount);
        Debug.Log("Total Enemies: " + enemies.Count);
        if (defeatedEnemiesCount == enemies.Count)
        {
            //defeatedEnemiesCount = 0;
            StartCoroutine(WaitAndInvoke());
        }
    }
}
