using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossRoomController : MonoBehaviour
{
    public UnityEvent onBossRoomOpen;
    public int finalRoom;
    public int[] finalRoomNumbers;

    void Awake()
    {
        // This will be called for every room instance, but that's okay
        finalRoom = finalRoomNumbers[Random.Range(0, finalRoomNumbers.Length)];
    }

    public void OpenBossRoom()
    {
        onBossRoomOpen.Invoke();
    }
}
