using UnityEngine;

[System.Serializable]
public class SaveData
{

    public Vector3 playerPosition;
    // Its a variable that will hold the player's position in the game world.
    // Это переменная, которая будет хранить позицию игрока в игровом мире.
    public string mapBoundary;
    //      I copy boundary system 
    // Its way to cut our game world to parts
    // When our Player trigger boundary collider - he pushes to new game zone 
    // So we need to save that, otherwise camera will always starts in 1st zone
    // Я скопировал систему границ
    // Это способ разделить наш игровой мир на части
    // Когда наш игрок пересекает коллайдер границы - он перемещается в новую игровую зону
    // Поэтому нам нужно это сохранить, иначе камера всегда будет начинать с первой зоны
}

