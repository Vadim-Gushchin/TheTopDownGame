using TMPro;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// That class handles the transition of the player between different map boundaries in a 2D game. When the player enters a trigger collider, it updates the camera's confiner to the new map boundary and adjusts the player's position based on the specified direction and additive position.
// Этот класс обрабатывает переход игрока между различными границами карты в 2D-игре. Когда игрок входит в триггерный коллайдер, он обновляет конфайнер камеры до новой границы карты и корректирует позицию игрока в зависимости от указанного направления и добавочной позиции.
public class MapTransation : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPosition;
    [SerializeField] float additivePos = 3f;
    CinemachineConfiner2D confiner;

    // An enumeration representing the possible directions for player movement during the map transition.
    // Перечисление, представляющее возможные направления движения игрока во время перехода карты.
    enum Direction { Up, Down, Left, Right, Teleport }


    private void Awake()
    {
        confiner = Object.FindAnyObjectByType<CinemachineConfiner2D>();
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FadeTransition(collision.gameObject);

            MapController_Manual.Instance?.HighlithArea(mapBoundry.name);
            MapController_Dynamic.Instance?.UpdateCurrentArea(mapBoundry.name);

        }
    }

    async void FadeTransition(GameObject player)
    {
        PauseController.SetPause(true);
        await FadeScript.Instance.FadeOut();

        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        await FadeScript.Instance.FadeIn();
        PauseController.SetPause(false);
    }

    // Updates the player's position based on the specified direction and additive position.
    // Обновляет позицию игрока в зависимости от указанного направления и добавочной позиции.
    private void UpdatePlayerPosition(GameObject player)
    {
        if (direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;
            case Direction.Down:
                newPos.y -= additivePos;
                break;
            case Direction.Left:
                newPos.x -= additivePos;
                break;
            case Direction.Right:
                newPos.x += additivePos;
                break;

        }
        player.transform.position = newPos;
    }

}
