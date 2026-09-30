using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    private void Start()
    {
        menuCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
            {
                PauseController.SetPause(false);
                return;
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetPause(menuCanvas.activeSelf);
        }
    }
}