using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] float minDropDistance = 3f;
    [SerializeField] float maxDropDistance = 4f;

    Transform originalParent;
    CanvasGroup canvasGroup;
    InventoryController inventoryController;


    void Start()
    {
        inventoryController = InventoryController.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(originalParent.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        InventorySlot dropSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();

        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;

            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<InventorySlot>();
            }
        }
        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();

        if (dropSlot == originalSlot)
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }

        if (dropSlot != null) 
        {
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();
                if (draggedItem.itemID == targetItem.itemID)
                {
                    targetItem.AddToStuck(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform invetoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(invetoryRect, mousePosition);
    }

    void DropItem(InventorySlot originalSlot)
    {
        Item dropItem = GetComponent<Item>();
        int quantity = dropItem.quantity;

        if (quantity > 1)
        {
            dropItem.RemoveFromStuck();

            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            quantity = 1;
        }
        else
        {
            originalSlot.currentItem = null;

        }


        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.Log("MISSING GAMEOBJECT WITH PLAYER TAG");
            return;
        }

        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPositon = (Vector2)playerTransform.position + dropOffset;

        GameObject dropItemOut = Instantiate(gameObject, dropPositon, Quaternion.identity);
        Item drioppedItem = dropItemOut.GetComponent<Item>();
        drioppedItem.quantity = quantity;

        dropItemOut.GetComponent<BounceEffect>().StartBounce();

        if ((quantity <= 1) && (originalSlot.currentItem == null))
        {
            Destroy(gameObject);
        }
        InventoryController.Instance.RebuldItemCount();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStuck();
        }
    }
    void SplitStuck()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.quantity <= 1)
            return;
        int quantity = item.quantity;
        int splitAmount = item.quantity / 2;
        if (splitAmount <= 0) return;

        item.RemoveFromStuck(splitAmount);

        GameObject newItem = item.CloneItem(splitAmount);

        if ((inventoryController == null) || (newItem == null)) return;

        foreach (Transform slotTransform in inventoryController.inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }

        //no emptyslot return to stuck
        item.AddToStuck(splitAmount);
        Destroy(newItem);
    }
}
