using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isShopItem;
    public InventorySlot originalInventorySlot;


    public void Initialise(bool shopItem) => isShopItem = shopItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isShopItem)
            {
                BuyItem();
            }
            else
            {
                SellItem();
            }
        }
    }
    private void BuyItem()
    {
        Item currentItem = GetComponent<Item>();
        ShopSlot currentShopSlot = GetComponentInParent<ShopSlot>();
        if (!currentItem || !currentShopSlot) return;

        if (CurrencyController.Instance.GetGold() < currentShopSlot.itemPrice)
        {
            Debug.Log("Not Enought GOLD bro");
            return;
        }

        GameObject itemPrefab = FindObjectOfType<ItemDictionary>().GetItemPrefabByID(currentItem.itemID);

        if (InventoryController.Instance.AddItemToInventory(itemPrefab))
        {
            CurrencyController.Instance.SpendGold(currentShopSlot.itemPrice);
            ShopController.Instance.UpdatePlyaerInventoryDisplay();
            ShopController.Instance.RemoveItemFromShop(currentItem.itemID, 1);
        }
        else
        {
            Debug.Log("InventoryFull bro");
        }
    }

    private void SellItem()
    {
        Item currentItem = GetComponent<Item>();
        ShopSlot currentShopSlot = GetComponentInParent<ShopSlot>();
        if (!currentItem || !currentShopSlot|| !originalInventorySlot) return;

        Item inventoryItem = originalInventorySlot.currentItem?.GetComponent<Item>();
        if (!inventoryItem) return;

        if (inventoryItem.quantity > 1) inventoryItem.RemoveFromStuck(1);
        else
        {
            Destroy(originalInventorySlot.currentItem);
            originalInventorySlot.currentItem = null;
        }

        InventoryController.Instance.RebuldItemCount();
        CurrencyController.Instance.AddGold(currentShopSlot.itemPrice);
        ShopController.Instance.UpdatePlyaerInventoryDisplay();
        ShopController.Instance.AddItemToShop(currentItem.itemID, 1);
    }
}
