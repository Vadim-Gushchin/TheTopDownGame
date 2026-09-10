using JetBrains.Annotations;
using Mono.Cecil.Cil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    [SerializeField] private List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
                itemPrefabs[i].itemID = i+1;
        }

        foreach (Item item in itemPrefabs)
            itemDictionary[item.itemID] = item.gameObject;

    }

    public GameObject GetItemPrefabByID(int itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out GameObject itemPrefab))
            return itemPrefab;

        else
        {
            Debug.LogWarning($"Item with ID {itemID} not found in the dictionary.");
            return null;
        }
    }
}


/* Code with comments in English and Russian:
public class ItemDictionary : MonoBehaviour
{
    This line declares a private serialized field of type List < Item > named itemPrefabs.This list will hold the prefabs of items that can be used in the game.The [SerializeField] attribute allows this field to be set in the Unity Inspector, even though it is private.
    Эта строка объявляет приватное сериализуемое поле типа List<Item> с именем itemPrefabs.Этот список будет содержать префабы предметов, которые могут использоваться в игре.Атрибут[SerializeField] позволяет установить это поле в инспекторе Unity, даже если оно приватное.
    [SerializeField] private List<Item> itemPrefabs;

    This line declares a private Dictionary<int, GameObject> named itemDictionary.This dictionary will be used to map item IDs(integers) to their corresponding GameObject prefabs.The key is the item ID, and the value is the GameObject prefab associated with that ID.
     Эта строка объявляет приватный словарь Dictionary<int, GameObject> с именем itemDictionary.Этот словарь будет использоваться для сопоставления идентификаторов предметов (целых чисел) с соответствующими префабами GameObject.Ключом является идентификатор предмета, а значением - связанный с ним префаб GameObject.
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {

        There we just initialize the itemDictionary as a new Dictionary<int, GameObject> to prepare it for storing item ID and prefab pairs.
        Тут мы просто инициализируем itemDictionary как новый Dictionary<int, GameObject>, чтобы подготовить его к хранению пар идентификаторов предметов и префабов.
        itemDictionary = new Dictionary<int, GameObject>();


        This loop iterates through the itemPrefabs list and assigns a unique itemID to each item based on its index in the list.The itemID is set to i + 1 to ensure that IDs start from 1 instead of 0.
        Этот цикл проходит по списку itemPrefabs и присваивает уникальный itemID каждому предмету на основе его индекса в списке.itemID устанавливается в i + 1, чтобы гарантировать, что идентификаторы начинаются с 1, а не с 0.
        for (int i = 0; i < itemPrefabs.Count; i++)
            {
                if (itemPrefabs[i] != null)
                {
                    Assign a unique ID to each item based on its index
                    Присваиваем уникальный идентификатор каждому предмету на основе его индекса
                    itemPrefabs[i].itemID = i + 1; 
                }

            }
        This loop iterates through the itemPrefabs list again and populates the itemDictionary with itemID as the key and the corresponding GameObject prefab as the value.
        Этот цикл снова проходит по списку itemPrefabs и заполняет itemDictionary, используя itemID в качестве ключа и соответствующий префаб GameObject в качестве значения.
        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.itemID] = item.gameObject;
        }
    }

    This method takes an integer itemID as input and attempts to retrieve the corresponding GameObject prefab from the itemDictionary.If the itemID is found, it returns the associated prefab; otherwise, it logs a warning message and returns null.
    Этот метод принимает целое число itemID в качестве входного параметра и пытается получить соответствующий префаб GameObject из itemDictionary.Если itemID найден, он возвращает связанный префаб; в противном случае он выводит предупреждающее сообщение и возвращает null.
    public GameObject GetItemPrefabByID(int itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out GameObject itemPrefab))
            return itemPrefab;
        else
        {
            Debug.LogWarning($"Item with ID {itemID} not found in the dictionary.");
            return null;
        }
    }
}
*/