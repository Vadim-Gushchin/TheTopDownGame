using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;

    public virtual void UseItem()
    {
        Debug.Log($" {itemName} was used");
    }

    public virtual void Pickup()
    {

        Sprite itemIcon= GetComponent<Image>().sprite;
        if(ItemPickUpUIController.Instance != null)
        {
            ItemPickUpUIController.Instance.ShowItemPickUp(itemName, itemIcon);
        }
        // Logic for picking up the item
        // Логика для поднятия предмета
    }
}
