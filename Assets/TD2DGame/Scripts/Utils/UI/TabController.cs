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
    //public void ActivateTab(int tabNumber)
    //{
    //    for (int i = 0; i < pages.Length; i++)
    //    {
    //        pages[i].SetActive(false);
    //        tabImages[i].color = Color.gray;
    //    }
    //    pages[tabNumber].SetActive(true);
    //    tabImages[tabNumber].color = Color.white;
    //}
    public void ActivateTab(int tabNumber)
    {

        // 1. Защита от неинициализированных массивов
        if (pages == null || tabImages == null)
        {
            Debug.LogError("[TabController] Массивы pages или tabImages не назначены в Inspector!");
            return;
        }

        // 2. Защита от несовпадения длин
        if (pages.Length != tabImages.Length)
        {
            Debug.LogError($"[TabController] Длина pages ({pages.Length}) != длины tabImages ({tabImages.Length})! Исправь в Inspector.");
            return;
        }

        // 3. Защита от выхода за границы массива
        if (tabNumber < 0 || tabNumber >= pages.Length)
        {
            Debug.LogWarning($"[TabController] tabNumber ({tabNumber}) вне диапазона [0..{pages.Length - 1}]");
            return;
        }

        // 4. Защита от null-элементов в массивах
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] == null)
            {
                Debug.LogError($"[TabController] pages[{i}] == null! Заполни массив в Inspector.");
                return;
            }
            if (tabImages[i] == null)
            {
                Debug.LogError($"[TabController] tabImages[{i}] == null! Заполни массив в Inspector.");
                return;
            }
        }

        // 5. Основная логика
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        }

        pages[tabNumber].SetActive(true);
        tabImages[tabNumber].color = Color.white;
    }
}


