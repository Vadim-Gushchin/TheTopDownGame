using TMPro;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    public GameObject currentItem;
    public int itemPrice;
    public TMP_Text priceText;
    public bool isShopSlot = true;
    // If true mean its slot on shop side if false = on player inventory side


    public void Awake()
    {
        if (!priceText)
        {
            priceText = transform.Find ("ItemPrice").GetComponent<TMP_Text>();
        }
    }

    public void UpdatePriceDisplay()
    {
        if (priceText && currentItem)
        {
            priceText.text = itemPrice.ToString();
        }
    }

    public void SetItem(GameObject item, int price)
    {
        currentItem = item;
        itemPrice = price;
        UpdatePriceDisplay();
    }
}
