using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    ItemList itemList;

    void Start()
    {
        itemList = FindObjectOfType<ItemList>();
        SpawnRandomItem();
        Destroy(this);
    }


    void SpawnRandomItem() 
    {    
        if (itemList.itemList.Count > 0)
        {
            int whichItem = UnityEngine.Random.Range(0, itemList.itemList.Count);
            Instantiate(itemList.itemList[whichItem], transform.position, Quaternion.identity);
            itemList.itemList.RemoveAt(whichItem);
        }
        else
        {
            Debug.LogWarning("No items left to spawn.");
        }
    }
}
