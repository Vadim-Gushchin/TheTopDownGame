using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int slotCount = 21;

    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject inventorySlotPrefab;
    public GameObject[] itemPrefabs;

    private void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
    }

    public bool AddItemToInventory(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();

            if (slot.currentItem == null && slot != null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true; // Item added successfully
            }
        }
        Debug.Log("Inventory is full. Cannot add item.");
        return false;
    }

    public List<InventorySaveData> GetInventorySaveData()
    {
        List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem != null)
            {
                Item currentItemInTheSlot = slot.currentItem.GetComponent<Item>();
                inventorySaveData.Add(new InventorySaveData { itemID = currentItemInTheSlot.itemID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        Debug.Log($"INVENTORY Вернул {inventorySaveData.Count} элементов");
        return inventorySaveData;
    }

    public void SetInvenotyItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
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
                    slot.currentItem = item;
                }
            }
        }
    }

}

/* Code with comments in English and Russian:
// This class manages the inventory system in the game. It creates inventory slots and populates them with items.
// Этот класс управляет системой инвентаря в игре. Он создает слоты инвентаря и заполняет их предметами.
public class InventoryController : MonoBehaviour
{
    [SerializeField] private int slotCount=21;
    // The number of inventory slots to create. This determines how many slots will be available in the inventory.
    // Количество слотов инвентаря для создания. Это определяет, сколько слотов будет доступно в инвентаре.

    public GameObject inventoryPanel;
    // The panel that will hold the inventory slots. This is a UI element where the slots will be instantiated.
    // Панель, которая будет содержать слоты инвентаря. Это элемент пользовательского интерфейса, где будут создаваться слоты.
    public GameObject inventorySlotPrefab;
    // The prefab for the inventory slot. This is a template for creating new slots in the inventory panel.
    // Префаб для слота инвентаря. Это шаблон для создания новых слотов в панели инвентаря.
   
    public GameObject[] itemPrefabs;
    // An array of item prefabs to populate the inventory slots. Each prefab represents a different item that can be placed in the inventory.
    // Массив префабов предметов для заполнения слотов инвентаря. Каждый префаб представляет собой разные предметы, которые могут быть помещены в инвентарь.

    private void Start()
    {
          itemDictionary = FindAnyObjectByType<ItemDictionary>();
           // This line finds an instance of the ItemDictionary class in the scene. The ItemDictionary is used to look up item prefabs by their unique IDs.
           // Эта строка находит экземпляр класса ItemDictionary в сцене. ItemDictionary используется для поиска префабов предметов по их уникальным идентификаторам.

        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, inventoryPanel.transform).GetComponent<InventorySlot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
        // This loop creates the specified number of inventory slots and populates them with items from the itemPrefabs array. Each slot is instantiated as a child of the inventoryPanel, and if there are enough item prefabs, they are instantiated within the corresponding slots.
        // Этот цикл создает указанное количество слотов инвентаря и заполняет их предметами из массива itemPrefabs. Каждый слот создается как дочерний элемент inventoryPanel, и если достаточно префабов предметов, они создаются внутри соответствующих слотов.
    }
}

    // This method retrieves the current state of the inventory, including the items in each slot and their corresponding slot indices. It returns a list of InventorySaveData objects that can be used for saving the inventory state.
   // Этот метод извлекает текущее состояние инвентаря, включая предметы в каждом слоте и их соответствующие индексы слотов. Он возвращает список объектов InventorySaveData, которые могут быть использованы для сохранения состояния инвентаря.
    public List<InventorySaveData> GetInventorySaveData() // This method retrieves the current state of the inventory, including the items in each slot and their corresponding slot indices. It returns a list of InventorySaveData objects that can be used for saving the inventory state.
    {
        List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot.currentItem != null)
            {
                Item currentItemInTheSlot = slot.currentItem.GetComponent<Item>();
                inventorySaveData.Add(new InventorySaveData { itemID = currentItemInTheSlot.itemID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        Debug.Log($"Вернул {inventorySaveData.Count + 1} элементов");
        return inventorySaveData;

    }

    // This method sets the inventory items based on a list of InventorySaveData objects. It clears the existing inventory slots, creates new slots, and populates them with the saved items according to their slot indices.
    // Этот метод устанавливает предметы инвентаря на основе списка объектов InventorySaveData. Он очищает существующие слоты инвентаря, создает новые слоты и заполняет их сохраненными предметами в соответствии с их индексами слотов.
    public void SetInvenotyItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
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
                    slot.currentItem = item;
                }
            }
        }
    }

}
*/