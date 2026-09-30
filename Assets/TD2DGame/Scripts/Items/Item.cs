using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalHelper;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public int quantity = 1;
    public int buyPrice = 10;
    [Range(0,1)]
    public float sellMultiplier = 0.5f;

    private TMP_Text quantityText;

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityText();
    }


    public virtual void UseItem()
    {
        quantity--;
        UpdateQuantityText();
        if (quantity <= 0)
        {
            Destroy(gameObject);
        }
        SoundEffectManager.PlaySoundEffect(SoundEffectConstants.UseItem);
        Debug.Log($" {itemName} was used");
    }

    public int GetSellPrice()
    {
        return Mathf.RoundToInt(buyPrice * sellMultiplier);
    }
    public virtual void ShowPickUp()
    {

        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickUpUIController.Instance != null)
        {
            ItemPickUpUIController.Instance.ShowItemPickUp(itemName, itemIcon);
        }
        // Logic for picking up the item
        // Логика для поднятия предмета
    }
    public void AddToStuck(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityText();
    }
    public int RemoveFromStuck(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        UpdateQuantityText();
        return quantity;
    }
    public GameObject CloneItem(int newAmount)
    {
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.quantity = newAmount;
        cloneItem.UpdateQuantityText();
        return clone;
    }

    public void UpdateQuantityText()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
}
