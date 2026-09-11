using UnityEngine;
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

        // Пытаемся получить компонент InentorySlot в точке сброса
        InentorySlot dropSlot = eventData.pointerEnter?.GetComponent<InentorySlot>();

        // Если dropSlot не найден напрямую, ищем его в родительских элементах
        if (dropSlot == null)
        {
            // Получаем объект, на который сброшен предмет
            GameObject dropItem = eventData.pointerEnter;

            // Если объект существует, ищем InentorySlot в его родительских элементах
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<InentorySlot>();
            }
        }

        // Получаем компонент InentorySlot исходного родителя
        InentorySlot originalSlot = originalParent.GetComponent<InentorySlot>();

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

