using UnityEngine;
using UnityEngine.UI;

// That class handles the tab switching functionality in the UI. It activates the selected tab and deactivates the others, changing their colors accordingly.
// Этот класс обрабатывает функциональность переключения вкладок в пользовательском интерфейсе. Он активирует выбранную вкладку и деактивирует остальные, изменяя их цвета соответственно.
public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    // An array of Image components representing the tabs in the UI. Each tab will have its color changed when activated or deactivated.
    // Массив компонентов Image, представляющих вкладки в пользовательском интерфейсе. Цвет каждой вкладки будет изменяться при активации или деактивации.
    public GameObject[] pages;
    // An array of GameObjects representing the pages associated with each tab. When a tab is activated, its corresponding page will be displayed.
    // Массив объектов GameObject, представляющих страницы, связанные с каждой вкладкой. Когда вкладка активируется, будет отображаться соответствующая ей страница.


    private void Start()
    {
        ActivateTab(0);
    }

    // Activates the specified tab and deactivates the others, changing their colors accordingly.
    // Активирует указанную вкладку и деактивирует остальные, изменяя их цвета соответственно.
    public void ActivateTab(int tabNumber)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        }
        pages[tabNumber].SetActive(true);
        tabImages[tabNumber].color = Color.white;
    }
}
