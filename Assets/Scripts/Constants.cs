using UnityEngine;

public static class SceneNames
{
    public const string MainScene = "MainScene";
    public const string MapSelectionScene = "MapSelectionScene";
    public const string GameScene = "GameScene";
}

public static class Paths
{
    public const string Prefabs = "Prefabs/";
    public const string Maps = Prefabs + "Maps/";
}

public static class Tags
{
    public const string WALL = "Wall";
    public const string SLAB = "Slab";
    public const string PLAYER = "Player";
    public const string SCENE_CONTROLLER = "SceneController";
    public const string SPAWN_POINT = "SpawnPoint";
    public const string CLEAR_POINT = "ClearPoint";
    public const string SWITCH_SLAB = "SwitchSlab";
    public const string FLOOR = "Floor";
    public const string WARP_SLAB = "WarpSlab";
    public const string OBJECT = "Object";
    public const string GEAR = "Gear";
    public const string BULLET = "Bullet";
}

public static class PrefabNames
{
    public const string PLAYER = "Player";
}

public static class Keys
{
    public const KeyCode layerBefore = KeyCode.Q;
    public const KeyCode layerNext = KeyCode.E;
    public const KeyCode boost = KeyCode.LeftShift;
    public const KeyCode respawn = KeyCode.Space;
    public const KeyCode quit = KeyCode.Escape;
}

public enum Difficulties
{
    Hard,
    Normal,
    Easy
}

public enum WarpModes
{
    Next,
    Before,
    None
}

public enum FloorTypes
{
    Normal,
    Ice,
    Water
}