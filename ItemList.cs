using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public List<GameObject> itemList;

    void Start()
    {
        GameObject[] itemPrefabs = Resources.LoadAll<GameObject>("Prefabs/Items");
        
        itemList = new List<GameObject>(itemPrefabs);
    }

}
