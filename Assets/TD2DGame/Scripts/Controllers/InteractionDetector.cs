using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteracteble interactebleInRange = null;
    public GameObject interactionIcon;
    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!IsTargetAlive())
            {
                ClearTarget();
                return;
            }

            interactebleInRange?.Interact();
            if (!interactebleInRange.CanInteract())
            {
                interactionIcon.SetActive(false);
            }
        }
        else return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteracteble interacteble) && interacteble.CanInteract())
        {
            interactebleInRange = interacteble;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteracteble interacteble) && interacteble == interactebleInRange)
        {
            ClearTarget();
        }
    }
    private void ClearTarget()
    {
        interactebleInRange = null;
        interactionIcon.SetActive(false);
    }
    private bool IsTargetAlive()
    {
        // Первая проверка — обычный null.
        // Вторая — ловит УНИЧТОЖЕННЫЙ объект: для интерфейсов "умный"
        // оператор == null не работает, поэтому приводим к UnityEngine.Object.
        return interactebleInRange != null && (interactebleInRange as UnityEngine.Object) != null;
    }
}