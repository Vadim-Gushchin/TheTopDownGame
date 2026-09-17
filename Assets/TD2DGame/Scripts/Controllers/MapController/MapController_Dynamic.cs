using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class is responsible for generating the map and updating the UI
//Этот класс отвечает за генерацию карты и обновление UI
public class MapController_Dynamic : MonoBehaviour
{    
    //There are references to the UI elements that will be used to display the map
    //Тут ссылки на UI-элементы, которые будут использоваться для отображения карты
    [Header("UI References")]
    public RectTransform mapParent;
    public GameObject areaPrefab;
    public RectTransform playerIcon;

    
    [Header("Colours")]
    public Color defaultColor = Color.gray;
    public Color currentAreaColor = Color.green;

    
    [Header("Map Settings")]
    public GameObject MapBounds; // its a prefab with a collider that defines the map boundaries
    public PolygonCollider2D initialArea;
    public float mapScale = 10f;

    private PolygonCollider2D[] mapAreas; // its an array of all the areas in the map
    private Dictionary<string, RectTransform> uiArea = new Dictionary<string, RectTransform>();

    public static MapController_Dynamic Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mapAreas = MapBounds.GetComponentsInChildren<PolygonCollider2D>();
    }
    public void GenerateMap(PolygonCollider2D newCurrentArea = null)
    {
        PolygonCollider2D currentArea = newCurrentArea != null ? newCurrentArea : initialArea;
        CleanMap();

        foreach (PolygonCollider2D area in mapAreas)
        {
            CreateAreaUI(area, area == currentArea);
        }
        MovePlayerIcon(currentArea.name);
    }

    public void UpdateCurrentArea(string newCurrentArea)
    {
        foreach(KeyValuePair<string, RectTransform> area in uiArea)
        {
            area.Value.GetComponent<Image>().color = area.Key == newCurrentArea ? currentAreaColor : defaultColor;
        }
        MovePlayerIcon(newCurrentArea);
    }

    private void CleanMap()
    {
        foreach (Transform child in mapParent)
        {
            Destroy(child.gameObject);
        }
        uiArea.Clear();
    }
    private void CreateAreaUI(PolygonCollider2D area, bool isCurrentArea)
    {
        GameObject areaImage = Instantiate(areaPrefab, mapParent);
        RectTransform rectTransform = areaImage.GetComponent<RectTransform>();

        Bounds bounds = area.bounds;

        rectTransform.sizeDelta = new Vector2(bounds.size.x * mapScale, bounds.size.y * mapScale);
        rectTransform.anchoredPosition = new Vector2(bounds.center.x * mapScale, bounds.center.y * mapScale);

        areaImage.GetComponent<Image>().color = isCurrentArea ? currentAreaColor : defaultColor;

        uiArea[area.name] = rectTransform;
    }

    private void MovePlayerIcon(string newCurrentArea)
    {
        if (uiArea.TryGetValue(newCurrentArea, out RectTransform areaUI))
        {
            playerIcon.anchoredPosition = areaUI.anchoredPosition;
        }
    }
}
