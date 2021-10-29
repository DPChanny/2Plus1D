using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private AudioClip clear;
    [SerializeField]
    private AudioClip dead;
    [SerializeField]
    private GameObject log;

    private void Start()
    {
        Public.isClear = false;
        Spawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(Keys.respawn))
        {
            Respawn();
        }
        if (Input.GetKeyDown(Keys.quit))
        {
            SceneManager.LoadScene(SceneNames.MapSelectionScene);
        }
        log.SetActive(Public.isDead || Public.isClear);
    }

    public void OnExit()
    {
        SceneManager.LoadScene(SceneNames.MapSelectionScene);
    }

    public void Dead()
    {
        Public.soundManager.Play(dead);
        Public.soundManager.StopBGM();
    }

    public void Clear()
    {
        FileManager.AddClearedMap(Public.currentMap.mapName);
        Public.soundManager.Play(clear);
        Public.soundManager.StopBGM();
    }

    public void Spawn()
    {
        ResetAll();
        Public.currentMap = Instantiate(Public.LoadMapPrefab(Public.selectedMap.mapName)).GetComponent<Map>();
        GameObject player = Instantiate((GameObject)Resources.Load(Paths.Prefabs + PrefabNames.PLAYER));
        Public.spawnPoint = new SpawnPoint(Public.currentMap.defaultSpawnPoint.position, Public.currentMap.currentLayerIndex);
        player.transform.position = Public.spawnPoint.spawnPosition;
        Public.soundManager.SetBGM(Public.currentMap.bgm);
        Public.soundManager.StartBGM();
    }

    public void Respawn()
    {
        ResetAll();
        Public.currentMap = Instantiate(Public.LoadMapPrefab(Public.selectedMap.mapName)).GetComponent<Map>();
        Public.currentMap.SetLayer(Public.spawnPoint.layerIndex);
        GameObject player = Instantiate((GameObject)Resources.Load(Paths.Prefabs + PrefabNames.PLAYER));
        player.transform.position = Public.spawnPoint.spawnPosition;
        Public.soundManager.SetBGM(Public.currentMap.bgm);
        Public.soundManager.StartBGM();
    }

    public void ResetAll()
    {

        try
        {
            Destroy(GameObject.FindGameObjectWithTag(Tags.PLAYER));
        }
        catch
        {

        }
        try
        {
            Destroy(Public.currentMap.gameObject);
        }
        catch
        {

        }
        Public.isDead = false;
        Public.isClear = false;
    }
}
