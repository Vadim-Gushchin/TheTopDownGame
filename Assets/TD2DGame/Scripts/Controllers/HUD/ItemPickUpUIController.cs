using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Collections;

public class ItemPickUpUIController : MonoBehaviour
{
    public static ItemPickUpUIController Instance { get; private set; }
    public GameObject itemPickUp;
    public int maxItemPickUpCount = 5;
    public float itemPickUpDuration=3f;

    private readonly Queue<GameObject> itemPickUpQueue = new Queue<GameObject>();
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowItemPickUp(string itemName, Sprite itemIcon)
    {
        GameObject newItemPickUp = Instantiate(itemPickUp, transform);
        newItemPickUp.GetComponentInChildren<TMP_Text>().text = itemName;
        Image itemImage = newItemPickUp.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = itemIcon;
        }

        itemPickUpQueue.Enqueue(newItemPickUp);
        if (itemPickUpQueue.Count > maxItemPickUpCount)
        {
            Destroy(itemPickUpQueue.Dequeue());
        }
       StartCoroutine(FadeOutAndDestroy(newItemPickUp));
    }

    private IEnumerator FadeOutAndDestroy(GameObject itemPickUp)
    {
        yield return new WaitForSeconds(itemPickUpDuration);
        if (itemPickUp == null) yield break;

        CanvasGroup canvasGroup = itemPickUp.GetComponent<CanvasGroup>();
        for (float timePassed = 0; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (itemPickUp == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }
        Destroy(itemPickUp);
    }
}
