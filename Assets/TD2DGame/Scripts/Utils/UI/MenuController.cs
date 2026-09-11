using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    private void Start()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            // Toggle the menu canvas on or off when the Tab key is pressed
            // Переключение меню на или выключение при нажатии клавиши Tab
        }
    }
}