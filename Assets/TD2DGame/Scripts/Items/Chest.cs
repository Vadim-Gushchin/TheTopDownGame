using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour, IInteracteble
{

    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject itemPrefab;
    public Sprite openedSprite;

    private void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    public void SetOpened(bool isOpened)
    {

        if (IsOpened=isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

    private void OpenChest()
    {
        SetOpened(true);
        SoundEffectManager.PlaySoundEffect("Chest");

        if (itemPrefab)
        {
            GameObject dropedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            dropedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }
}
