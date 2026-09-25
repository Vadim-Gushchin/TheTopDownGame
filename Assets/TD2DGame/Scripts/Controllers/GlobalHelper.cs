using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalHelper
{
    public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; 
        //Это уникальный идентификатор для каждого объекта в сцене
        //Он состоит из имени сцены, координаты X и Y объекта.
        //Таким образом, если два объекта имеют одинаковые координаты X и Y, то они будут иметь одинаковый идентификатор. 
        //Как легкое решение, но не идеальное. Т.к. если в сцене много объектов, то идентификаторы могут пересекаться.
    }

    public static class AnimatorConstants
    {
        public const string IsWalking = "IsWalking";
        public const string LastInputX = "LastInputX";
        public const string LastInputY = "LastInputY";
        public const string CurrentInputX = "CurrentInputX";
        public const string CurrentInputY = "CurrentInputY";
    }

    public static class SoundEffectConstants
    {
        public const string FootSteps = "FootSteps";
        public const string PickUp = "PickUp";
        public const string DropItem = "DropItem";
        public const string Chest = "Chest";
        public const string UseItem = "UseItem";
    }
}

