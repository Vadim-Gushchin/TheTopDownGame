using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] float minDropDistance =3f;
    [SerializeField] float maxDropDistance =4f;

    Transform originalParent;
    CanvasGroup canvasGroup;


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(originalParent.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        InventorySlot dropSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();

        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;

            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<InventorySlot>();
            }
        }
        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
            }
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform invetoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(invetoryRect, mousePosition);
    }

    void DropItem(InventorySlot originalSlot)
    {
        originalSlot.currentItem = null;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.Log("MISSING GAMEOBJECT WITH PLAYER TAG");
            return;
        }

        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPositon = (Vector2)playerTransform.position + dropOffset;

        GameObject dropItem = Instantiate(gameObject, dropPositon, Quaternion.identity);
      
        dropItem.GetComponent<BounceEffect>().StartBounce();

        Destroy(gameObject);

    }
}

/*using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Класс обрабатывает перетаскивание предметов в системе инвентаря.
/// Реализует интерфейсы IDragHandler, IBeginDragHandler и IEndDragHandler для управления поведением перетаскивания и сброса предметов.
/// </summary>
public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Исходный родитель перетаскиваемого предмета
    Transform originalParent;

    // Компонент CanvasGroup для управления видимостью и взаимодействием
    CanvasGroup canvasGroup;

    void Start()
    {
        // Получаем компонент CanvasGroup текущего объекта
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // Сохраняем исходного родителя для возврата предмета в исходную позицию
        originalParent = transform.parent;

        // Перемещаем предмет в корневой элемент Canvas для свободного перемещения
        transform.SetParent(originalParent.root);

        // Отключаем блокировку raycasts, чтобы предмет не блокировал события мыши
        canvasGroup.blocksRaycasts = false;

        // Делаем предмет полупрозрачным для визуальной обратной связи
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Обновляем позицию предмета в соответствии с позицией мыши
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Вернуть блокировку raycasts для взаимодействия с UI
        canvasGroup.blocksRaycasts = true;

        // Восстановить полную непрозрачность
        canvasGroup.alpha = 1f;

        // Пытаемся получить компонент InventorySlot в точке сброса
        InventorySlot dropSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();

        // Если dropSlot не найден напрямую, ищем его в родительских элементах
        if (dropSlot == null)
        {
            // Получаем объект, на который сброшен предмет
            GameObject dropItem = eventData.pointerEnter;

            // Если объект существует, ищем InventorySlot в его родительских элементах
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<InventorySlot>();
            }
        }

        // Получаем компонент InventorySlot исходного родителя
        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();

        // Если найлось место сброса (инвентарный слот), обменяем предметы или поместим новый предмет
        if (dropSlot != null)
        {
            // Если в целевом слоте уже есть предмет, то обменяем его на исходный
            if (dropSlot.currentItem != null)
            {
                // Перемещаем предмет из целевого слота в исходный слот
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                // Иначе очищаем исходный слот
                originalSlot.currentItem = null;
            }

            // Помещаем перетаскиваемый предмет в целевой слот
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // Если целевой слот не найден, возвращаем предмет в исходную позицию
            transform.SetParent(originalParent);
        }

        // Сбрасываем позицию предмета относительно родителя
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}


 */