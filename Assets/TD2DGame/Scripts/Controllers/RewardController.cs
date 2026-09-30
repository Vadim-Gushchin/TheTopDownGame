using UnityEngine;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance { get; private set; }
    private ItemDictionary itemDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            itemDictionary = FindAnyObjectByType<ItemDictionary>();
        }

        else Destroy(gameObject);
    }

    public void GiveQuestReard(Quest quest)
    {
        if(quest?.questRewards==null) return;

        foreach(var reward in quest.questRewards)
        {
            switch (reward.rewardType)
            {
                case RewaedType.Gold: break;
                case RewaedType.Item: GiveItemReward(reward.rewardID, reward.amount);
                    break;
                case RewaedType.Expirience:break;
                case RewaedType.Custom:break;
            }
        }
    }

    public void GiveItemReward(int itemID,int amount)
    {
        Debug.Log($"[RewardController] Выдача награды: itemID={itemID}, amount={amount}");

        var itemPrefab = itemDictionary?.GetItemPrefabByID(itemID);

        if(itemPrefab == null) return;

        for (int i = 0; i < amount; i++)
        {
            if (!InventoryController.Instance.AddItemToInventory(itemPrefab))
            {
                GameObject dropItem = Instantiate(itemPrefab, transform.position+Vector3.down, Quaternion.identity);
                dropItem.GetComponent<BounceEffect>().StartBounce();
                
            }
            else
            {
                itemPrefab.GetComponent<Item>().ShowPickUp();
              
            }

        }

    }
}
