using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private GameObject itemSlots;
    public Sprite revolverSprite;
    [SerializeField] private int itemSlotSize = 50;
    private int cols;
    private int itemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        itemSlots = GameObject.Find("ItemSlots");
        
        InventoryController inventory = GetComponent<InventoryController>();
        int inventoryWidth = (int)inventory.transform.GetComponent<RectTransform>().rect.width;
        cols = inventoryWidth / (int)itemSlotSize;
    }

    public void AddItemToInventory(string itemName, Sprite itemSprite)
    {
        GameObject slot = new GameObject(itemName);

        RectTransform trans = slot.AddComponent<RectTransform>();
        trans.transform.SetParent(itemSlots.transform);
        trans.localScale = Vector3.one;
        trans.pivot = new Vector2(0f, 1f);
        trans.anchorMin = new Vector2(0f, 1f);
        trans.anchorMax = new Vector2(0f, 1f);

        int row = itemCount / cols;
        int col = itemCount % cols;

        trans.anchoredPosition = new Vector2(col * itemSlotSize, -row * itemSlotSize);
        trans.sizeDelta = new Vector2(itemSlotSize, itemSlotSize);

        Image image = slot.AddComponent<Image>();
        image.sprite = itemSprite;
        slot.transform.SetParent(itemSlots.transform);

        itemCount++;
    }
}
