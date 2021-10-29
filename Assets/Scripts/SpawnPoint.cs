using UnityEngine;

public class SpawnPoint
{
    public Vector2 spawnPosition;
    public int layerIndex;

    public SpawnPoint(Vector2 _spawnPosition, int _layerIndex)
    {
        spawnPosition = _spawnPosition;
        layerIndex = _layerIndex;
    }
}
