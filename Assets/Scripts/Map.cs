using UnityEngine;

public class Map : MonoBehaviour
{
    public string mapName
    {
        get
        {
            return name.Replace("(Clone)", "");
        }
    }

    [SerializeField]
    private Layer[] layers;

    [HideInInspector]
    public int currentLayerIndex = 0;

    public Transform defaultSpawnPoint;

    [SerializeField]
    private int defaultLayerIndex = 0;

    public AudioClip bgm;

    public Difficulties difficulty;

    private void Awake()
    {
        Init();
    }

    public int GetCountOfLayer()
    {
        return layers.Length;
    }

    public void Init()
    {
        SetLayer(defaultLayerIndex);
    }

    public void NextLayer()
    {
        if(currentLayerIndex == layers.Length - 1)
        {
            SetLayer(0);
        }
        else
        {
            SetLayer(currentLayerIndex + 1);
        }
    }

    public void BeforeLayer()
    {
        if (currentLayerIndex == 0)
        {
            SetLayer(layers.Length - 1);
        }
        else
        {
            SetLayer(currentLayerIndex - 1);
        }
    }

    public void SetLayer(int _index)
    {
        currentLayerIndex = _index;
        InitLayer();
    }

    public Layer GetLayer()
    {
        return layers[currentLayerIndex];
    }

    private void InitLayer()
    {
        for(int i = 0; i < layers.Length; i++)
        {
            layers[i].gameObject.SetActive(i == currentLayerIndex);
        }
    }
}
