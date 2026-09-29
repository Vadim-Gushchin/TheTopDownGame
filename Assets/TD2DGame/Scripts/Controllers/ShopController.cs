using System;
using TMPro;
using UnityEngine;
using UnityPipeline.Microsoft.CodeAnalysis.CSharp.Syntax;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance { get; private set; }

    [Header("UI")]
    public GameObject shopPanel;
    public Transform shopInventoryGrid, playerInventoryGrid;
    public GameObject shopSlotPrefab;
    public TMP_Text playerMoneyText, shopTitleText;

    private ItemDictionary itemDictionary;
    private ShopNPC currentShop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        shopPanel.SetActive(false);

        if (CurrencyController.Instance != null)
        {
            CurrencyController.Instance.OnGoldChanged += UpdateMoneyDisplay;
            UpdateMoneyDisplay(CurrencyController.Instance.GetGold());
        }

    }

    private void UpdateMoneyDisplay(int amount)
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = amount.ToString();
        }
    }

    public void OpenShop(ShopNPC shop)
    {
        currentShop = shop;
        shopPanel.SetActive(true);
        if (shopTitleText != null)
        {
            shopTitleText.text = shop.shopKeeperName += "'s Shop";
        }
        UpdateShopInventorDisplay();
        UpdatePlyaerInventoryDisplay();
        PauseController.SetPause(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        currentShop = null;
        PauseController.SetPause(false);
    }

    public void UpdatePlyaerInventoryDisplay()
    {

        if (InventoryController.Instance == null) return;
        foreach (Transform child in playerInventoryGrid) Destroy(child.gameObject);

        foreach (Transform slotTransform in InventoryController.Instance.inventoryPanel.transform)
        {
            InventorySlot inventorySlot = slotTransform.GetComponent<InventorySlot>();
            if (inventorySlot?.currentItem != null)
            {
                Item orginalItem = inventorySlot.currentItem.GetComponent<Item>();
                CreateShopSLot(playerInventoryGrid, orginalItem.itemID, orginalItem.quantity, false, inventorySlot);
            }
        }
    }
    public void UpdateShopInventorDisplay()
    {
        if (currentShop == null) return;
        foreach (Transform child in shopInventoryGrid) Destroy(child.gameObject);

        foreach (var stockItem in currentShop.GetShopStock())
        {
            if (stockItem.quantity <= 0) continue;
            CreateShopSLot(shopInventoryGrid, stockItem.itemID, stockItem.quantity, true);
        }
    }

    private void CreateShopSLot(Transform grid, int itemID, int quantity, bool isShop, InventorySlot originalSlot = null)
    {
        GameObject slotObj = Instantiate(shopSlotPrefab, grid);
        GameObject itemPrefab = itemDictionary.GetItemPrefabByID(itemID);
        if (itemPrefab == null) return;

        GameObject itemInstnace = Instantiate(itemPrefab, slotObj.transform);
        itemInstnace.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        Item item = itemInstnace.GetComponent<Item>();
        item.quantity = quantity;
        item.UpdateQuantityText();

        int price = isShop ? item.buyPrice : item.GetSellPrice();

        ShopSlot slot = slotObj.GetComponent<ShopSlot>();
        slot.isShopSlot = isShop;
        slot.SetItem(itemInstnace, price);

        ItemDragHandler dragHandler = itemInstnace.GetComponent<ItemDragHandler>();
        if (dragHandler) dragHandler.enabled = false;

        ShopItemHandler handler = itemInstnace.GetComponent<ShopItemHandler>();
        handler.Initialise(isShop);
        if (!isShop) handler.originalInventorySlot = originalSlot;
    }

    public void AddItemToShop(int itemID,int quantity)
    {
        if (!currentShop) return;
        currentShop.AddToStock(itemID, quantity);
        UpdateShopInventorDisplay();
    }

    public bool RemoveItemFromShop(int itemID,int quantity)
    {
        if (!currentShop) return false;

        bool success = currentShop.RemoveFromStock(itemID, quantity);
        if (success) UpdateShopInventorDisplay();
        return success;
    }
}
