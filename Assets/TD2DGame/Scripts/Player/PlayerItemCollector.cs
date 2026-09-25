using UnityEngine;
using static GlobalHelper;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    private void Start()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                bool itemAdded = inventoryController.AddItemToInventory(collision.gameObject);
                if(itemAdded) 
                {
                    item.ShowPickUp();
                    SoundEffectManager.PlaySoundEffect(SoundEffectConstants.PickUp);
                    Destroy(collision.gameObject);
                }
            }
        }
    }


}
