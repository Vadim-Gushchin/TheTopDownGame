using UnityEngine;

// This class manages the inventory system in the game. It creates inventory slots and populates them with items.
// Этот класс управляет системой инвентаря в игре. Он создает слоты инвентаря и заполняет их предметами.
public class inventoryController : MonoBehaviour
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


    //
    private void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            InentorySlot slot = Instantiate(inventorySlotPrefab, inventoryPanel.transform).GetComponent<InentorySlot>();
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
