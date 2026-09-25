using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    private Chest[] chests;

    private void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindAnyObjectByType<InventoryController>();
        hotbarController = FindAnyObjectByType<HotbarController>();
        chests = FindObjectsOfType<Chest>();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData()
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventorySaveData(),
            hotbarSaveData = hotbarController.GetHotbarSaveData(),
            chestSaveData = GetChestState(),
            questsProgressSaveData = QuestController.Instance.activateQuest
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));

        Debug.Log($"Игры сохранена. \n Директория путь сохранения: {Application.persistentDataPath}");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            PolygonCollider2D saveMapBoundary = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = saveMapBoundary;


            MapController_Manual.Instance?.HighlithArea(saveData.mapBoundary);
            MapController_Dynamic.Instance?.GenerateMap(saveMapBoundary);

            inventoryController.SetInvenotyItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);


            LoadChestStates(saveData.chestSaveData);

            QuestController.Instance.LoadQuestProgress(saveData.questsProgressSaveData);
            PauseController.SetPause(false);

            Debug.Log($"Игра Загружена.");
        }
        else
        {
            SaveGame();
            inventoryController.SetInvenotyItems(new List<InventorySaveData>());
            hotbarController.SetHotbarItems(new List<InventorySaveData>());
            MapController_Dynamic.Instance?.GenerateMap();
        }
    }

    private List<ChestSaveData> GetChestState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpenned = chest.IsOpened
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.Find(c => c.chestID == chest.ChestID);
            if (chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpenned);
            }
        }

    }
}


/* Code with comments in English and Russian:
 * 
 * Its a class that handles saving and loading the player's position and the current map boundary to and from a JSON file..
//Этот класс обрабатывает сохранение и загрузку позиции игрока и текущей границы карты в JSON-файл и из него.
public class SaveController : MonoBehaviour
{
    //That string variable will hold the path to the save file on the device's persistent data path.
    //Эта строковая переменная будет хранить путь к файлу сохранения в постоянном каталоге данных устройства.
    private string saveLocation; 


    private void Start()
    {
        saveLocation =  Path.Combine(Application.persistentDataPath, "saveData.json");

        Save location its a file called saveData.json in the persistent data path of the application.
        Местоположение сохранения - это файл с именем saveData.json в постоянном каталоге данных приложения.

         inventoryController = FindAnyObjectByType<InventoryController>();
         Just find the first Instance of the InventoryController class in the scene and assign it to the inventoryController variable.
         Просто находим  экземпляр класса InventoryController в сцене и присвойте его переменной inventoryController.

        LoadGame();

    }

    // Save the player's position and the current map boundary to a JSON file
    //Сохранить позицию игрока и текущую границу карты в JSON-файл
    public void SaveGame()
    {
        SaveData saveData = new SaveData()
        //Create a new Instance of the SaveData class and populate it with the player's position and the current map boundary
        //Создайте новый экземпляр класса SaveData и заполните его позицией игрока и текущей границей карты
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name
            inventorySaveData = inventoryController.GetInventorySaveData(),
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        //Write the JSON representation of the SaveData object to the save file
        //Запишите JSON-представление объекта SaveData в файл сохранения
    }

    // Load the player's position and the current map boundary from a JSON file
    //Загрузить позицию игрока и текущую границу карты из JSON-файла
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        //Check if the save file exists
        //Проверить, существует ли файл сохранения
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            inventoryController.SetInventoryItems(saveData.inventorySaveData);
        }
        else
        {
            SaveGame();
        }
    }
}
 */