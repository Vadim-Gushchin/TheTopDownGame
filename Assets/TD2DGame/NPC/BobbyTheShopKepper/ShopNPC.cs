using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteracteble
{
    public string shopID = "Shop_number_1"; // there we can add generation by Shop_number + id library of shops by name;
    public string shopKeeperName = "Bobby"; // there we can add name of shop keeper by NameDictiory<int,string> where int its ID, or name getter by objectName;

    public List<ShopStockItem> defoultShopStock = new();
    private List<ShopStockItem> currentShopStock = new();

    private bool isInitialized = false;


    [System.Serializable]
    public class ShopStockItem
    {
        public int itemID;
        public int quantity;
    }


    void Start()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        if (isInitialized) return;

        currentShopStock = new List<ShopStockItem>();
        foreach (var item in defoultShopStock)
        {
            currentShopStock.Add(new ShopStockItem
            {
                itemID = item.itemID,
                quantity = item.quantity

            });
        }
        isInitialized = true;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (ShopController.Instance == null) return;

        if (ShopController.Instance.shopPanel.activeSelf)
        {
            ShopController.Instance.CloseShop();
        }
        else
        {
            ShopController.Instance.OpenShop(this);
        }
    }

    public List<ShopStockItem> GetShopStock()
    {
        return currentShopStock;
    }

    public void SetShopStock(List<ShopStockItem> savedShopStock)
    {
        currentShopStock = savedShopStock;
    }

    public void AddToStock(int tempItemID,int tempQuantity)
    {
        ShopStockItem existing = currentShopStock.Find(currenSS => currenSS.itemID == tempItemID);
        if (existing != null)
        {
            existing.quantity += tempQuantity;
        }
        else
        {
            currentShopStock.Add(new ShopStockItem { itemID = tempItemID, quantity = tempQuantity });
        }
    }

    public bool RemoveFromStock(int tempItemID, int tempQuantity)
    {
        ShopStockItem existing = currentShopStock.Find(currenSS => currenSS.itemID == tempItemID);
        if (existing != null && existing.quantity >= tempQuantity)
        {
            existing.quantity -= tempQuantity;
            return true;
        }
        return false;
    }
}
