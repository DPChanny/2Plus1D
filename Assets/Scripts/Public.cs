using System.IO;
using UnityEngine;

public class Public
{
    public static Map selectedMap;
    public static Map currentMap;
    public static LogManager logManager;
    public static SoundManager soundManager;

    public static bool init = false;

    public static SpawnPoint spawnPoint;

    public static bool isClear;
    public static bool isDead;

    public static GameObject LoadMapPrefab(string _mapName)
    {
        return (GameObject)Resources.Load(Paths.Maps + _mapName + "/" + _mapName);
    }

}
