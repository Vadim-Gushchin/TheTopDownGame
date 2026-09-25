using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int slotCount = 21;

    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject inventorySlotPrefab;
    public GameObject[] itemPrefabs;

    public static InventoryController Instance { get; private set; }
    Dictionary<int, int> itemsCountCache = new();

    public event Action OnInventoryChanged;


    private void Awake()
    {
        if ((Instance != null) && (Instance == this))
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        RebuldItemCount();
    }

    public bool AddItemToInventory(GameObject itemPrefab)
    {
        //This 2 lines are to check if the item is in the itemDictionary
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        //chek if the item is in the inventory already have the example
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem != null && slot != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.itemID == itemToAdd.itemID) // Эта строчка проверяет, совпадает ли идентификатор предмета в слоте с идентификатором предмета, который нужно добавить.
                {
                    slotItem.AddToStuck();
                    RebuldItemCount();
                    return true; // Item added to stuck successfully
                }
            }
        }

        //look for empty
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem == null && slot != null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                RebuldItemCount();
                return true; // Item added successfully
            }
        }
        Debug.Log("Inventory is full. Cannot add item.");
        return false;
    }

    public void RebuldItemCount()
    {
        itemsCountCache.Clear();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem != null)
            {
                Item currentItemInTheSlot = slot.currentItem.GetComponent<Item>();
                if (currentItemInTheSlot != null)
                {
                    itemsCountCache[currentItemInTheSlot.itemID] = itemsCountCache.GetValueOrDefault(currentItemInTheSlot.itemID, 0) + currentItemInTheSlot.quantity;
                }
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public Dictionary<int, int> GetItemCounts() => itemsCountCache;

    public List<InventorySaveData> GetInventorySaveData()
    {
        List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem != null)
            {
                Item currentItemInTheSlot = slot.currentItem.GetComponent<Item>();
                inventorySaveData.Add(new InventorySaveData
                {
                    itemID = currentItemInTheSlot.itemID,
                    slotIndex = slotTransform.GetSiblingIndex(),
                    quantity = currentItemInTheSlot.quantity
                });
            }
        }
        Debug.Log($"INVENTORY Вернул {inventorySaveData.Count} элементов");
        return inventorySaveData;
    }

    public void SetInvenotyItems(List<InventorySaveData> inventorySaveData)
    {
        for (int i = inventoryPanel.transform.childCount - 1; i >= 0; i--)
        {

            DestroyImmediate(inventoryPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(inventorySlotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData savedData in inventorySaveData)
        {
            if (savedData.slotIndex < slotCount)
            {
                InventorySlot slot = inventoryPanel.transform.GetChild(savedData.slotIndex).GetComponent<InventorySlot>();
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
        RebuldItemCount();
    }

    public void ReRemoveItemFromInventory(int itemId, int amountToRemove)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (amountToRemove <= 0) break;

            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem?.GetComponent<Item>() is Item item && item.itemID == itemId)
            {
                int removed = Math.Min(amountToRemove, item.quantity);
                item.RemoveFromStuck(removed);
                amountToRemove -= removed;

                if (item.quantity <= 0)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }
        RebuldItemCount();
    }

}
