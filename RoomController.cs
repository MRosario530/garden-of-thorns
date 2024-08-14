using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RoomController : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public int gunflowerMinCount;
        public int gunflowerMaxCount;

        public int bearMinCount;
        public int bearMaxCount;

        public int dragonMinCount;
        public int dragonMaxCount;
    }


    [SerializeField] private Wave[] waves;
    [SerializeField] private int waveIndex;
    public float spawnRadius;
    private int currentEnemyCount;
    public float itemSpawnHeight;

    public BossRoomController bossRoomController;

    [Header("Prefabs")]
    private GameObject gunflowerPrefab;
    private GameObject bearPrefab;
    private GameObject dragonPrefab;

    [Header("Audio")]
    public AudioSource ambientMusic;
    public AudioSource battleMusic;
    public AudioSource roomAudioSource;
    public AudioClip roomStartClip;
    public AudioClip roomClearClip;

    public int roomNumber;
    bool isFinalRoom;
    [SerializeField] UnityEvent onRoomClear;
    ItemList itemList;
    GameObject enemies;
    private Transform player;

    void Awake()
    {
        isFinalRoom = (roomNumber == bossRoomController.finalRoom);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemies = new GameObject("Enemies");
        enemies.transform.parent = transform;
        gunflowerPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Gunflower");
        bearPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Bear");
        dragonPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Dragon");
        ambientMusic.Stop();
        battleMusic.Play();
        

        itemList = FindObjectOfType<ItemList>();

        waveIndex = 0;
        SpawnWave();
    }

    void SpawnRandomItem()
    {
        if (itemList.itemList.Count > 0)
        {
            int whichItem = UnityEngine.Random.Range(0, itemList.itemList.Count);
            Instantiate(itemList.itemList[whichItem], transform.position + Vector3.up * itemSpawnHeight, Quaternion.identity);
            itemList.itemList.RemoveAt(whichItem);
        }
        else
        {
            Debug.LogWarning("No items left to spawn.");
        }
    }

    public void SpawnGunflowers(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPoint;
            if (RandomPoint(transform.position, spawnRadius, out spawnPoint) && Vector3.Distance(spawnPoint, player.transform.position) > 4f)
            {
                currentEnemyCount++;
                Quaternion quaternion = Quaternion.LookRotation(player.position - spawnPoint);
                GameObject enemy = Instantiate(gunflowerPrefab, spawnPoint + Vector3.up * 5, quaternion);
                enemy.transform.parent = enemies.transform;
                enemy.GetComponent<Enemy>().roomController = this;
            }
        }
    }

    public void SpawnBears(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPoint;
            if (RandomPoint(transform.position, spawnRadius, out spawnPoint) && Vector3.Distance(spawnPoint, player.transform.position) > 4f)
            {
                currentEnemyCount++;
                Quaternion quaternion = Quaternion.LookRotation(player.position - spawnPoint);
                GameObject enemy = Instantiate(bearPrefab, spawnPoint, quaternion);
                enemy.transform.parent = enemies.transform;
                enemy.GetComponent<Enemy>().roomController = this;
            }
        }
    }

    public void SpawnDragons(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPoint;

            // y = 3 is the level of the dragon's plane
            Vector3 center = new Vector3(transform.position.x, 3, transform.position.z);

            if (RandomPoint(center, spawnRadius, out spawnPoint))
            {
                currentEnemyCount++;
                Quaternion quaternion = Quaternion.LookRotation(player.position - spawnPoint);
                GameObject enemy = Instantiate(dragonPrefab, spawnPoint, quaternion);
                enemy.transform.parent = enemies.transform;
                enemy.GetComponent<Enemy>().roomController = this;
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 45; i++)
        {
            Vector2 randomPointInUnitCircle2D = UnityEngine.Random.insideUnitCircle;
            Vector3 randomPointInUnitCircle3D = new Vector3(randomPointInUnitCircle2D.x, 0, randomPointInUnitCircle2D.y);
            Vector3 randomPoint = center + randomPointInUnitCircle3D * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) && Vector3.Distance(hit.position, player.transform.position) > 4.5f)
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public void EnemyDefeated()
    {
        currentEnemyCount--;

        if (currentEnemyCount == 0)
        {
            if (waveIndex == waves.Length)
            {
                onRoomClear.Invoke();
                battleMusic.Stop();
                ambientMusic.Play();
                roomAudioSource.PlayOneShot(roomClearClip);
                SpawnRandomItem();
                if (isFinalRoom)
                {
                    bossRoomController.OpenBossRoom();
                }
            }
            else
            {
                StartCoroutine(SpawnWaveAfterDelay(3f));
            }
        }

    }


    void SpawnWave()
    {
        roomAudioSource.PlayOneShot(roomStartClip);
        Wave wave = waves[waveIndex];
        SpawnGunflowers(UnityEngine.Random.Range(wave.gunflowerMinCount, wave.gunflowerMaxCount + 1));
        SpawnBears(UnityEngine.Random.Range(wave.bearMinCount, wave.bearMaxCount + 1));
        SpawnDragons(UnityEngine.Random.Range(wave.dragonMinCount, wave.dragonMaxCount + 1));
        waveIndex++;
    }

    private IEnumerator SpawnWaveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnWave();
    }

    void OnDrawGizmosSelected()
    {
        // Draw spawn radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.blue;
        /*
        if (isFinalRoom)
        {
            Handles.Label(transform.position, "Room " + roomNumber, style);
        }
        else
        {
            Handles.Label(transform.position, "Room " + roomNumber);
        }
        */
    }
}
