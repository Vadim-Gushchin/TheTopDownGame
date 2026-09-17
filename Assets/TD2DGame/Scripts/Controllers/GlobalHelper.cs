using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalHelper 
{
    public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; //chest 3_4
    }
}


//using UnityEngine;
//using UnityEngine.InputSystem;

//public class InteractionDetector : MonoBehaviour
//{
//    private IInteracteble interactebleInRange = null;
//    public GameObject interactionIcon;
//    void Start()
//    {
//        interactionIcon.SetActive(false);
//    }

//    public void OnInteract(InputAction.CallbackContext context)
//    {
//        if (context.performed)
//        {
//            interactebleInRange?.Interact();
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.TryGetComponent(out IInteracteble interacteble) && interacteble.CanInteract())
//        {
//            interactebleInRange = interacteble;
//            interactionIcon.SetActive(true);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.TryGetComponent(out IInteracteble interacteble) && interacteble == interactebleInRange)
//        {
//            interactebleInRange = null;
//            interactionIcon.SetActive(false);
//        }
//    }

//}
