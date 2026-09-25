using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{

    public GameObject hotbarPanel;
    public GameObject hotbarSlotPrefab;
    public int numberOfSlots = 10;
    private ItemDictionary itemDictionary;

    private Key[] hotbarKeys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        hotbarKeys = new Key[numberOfSlots];
        for (int i = 0; i < numberOfSlots; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
                UseItemInSlot(i);
        }
    }

    void UseItemInSlot(int slotIndex)
    {
        InventorySlot slot = hotbarPanel.transform.GetChild(slotIndex).GetComponent<InventorySlot>();
        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }


    public List<InventorySaveData> GetHotbarSaveData()
    {
        List<InventorySaveData> hotbarData = new List<InventorySaveData>();

        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem != null)
            {
                Item currentItemInTheSlot = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new InventorySaveData { itemID = currentItemInTheSlot.itemID,
                    slotIndex = slotTransform.GetSiblingIndex(), 
                    quantity = currentItemInTheSlot.quantity });
            }
        }
        Debug.Log($"HOTBAR Вернул {hotbarData.Count} элементов");
        return hotbarData;
    }

    public void SetHotbarItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfSlots; i++)
        {
            Instantiate(hotbarSlotPrefab, hotbarPanel.transform);
        }

        foreach (InventorySaveData savedData in inventorySaveData)
        {
            if (savedData.slotIndex < numberOfSlots)
            {
                InventorySlot slot = hotbarPanel.transform.GetChild(savedData.slotIndex).GetComponent<InventorySlot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefabByID(savedData.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    Item itemComponent = item.GetComponent<Item>();
                    if (itemComponent != null && savedData.quantity > 1)
                    {
                        itemComponent.quantity = savedData.quantity;
                        itemComponent.UpdateQuantityText();
                    }
                    slot.currentItem = item;
                }
            }
        }
    }

}
