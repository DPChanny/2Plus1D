using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MapSelectionScene : MonoBehaviour
{
    private bool isMapSelected = false;

    [SerializeField]
    private Category[] categories;

    [SerializeField]
    private Text TextUI_Start;

    [SerializeField]
    private ScrollViewManager ScrollView_Maps;

    [SerializeField]
    private ScrollViewManager ScrollView_Categories;

    [SerializeField]
    private RectTransform ScrollAnimationHolder;

    [SerializeField]
    private GameObject CategoryButtonUI;
    [SerializeField]
    private GameObject MapButtonUI;

    private readonly float animationSpeed = 5f;

    private Category currentCategory;

    [SerializeField]
    private AudioClip bgm;

    public void Start()
    {
        isMapSelected = false;
        ScrollView_Categories.ResetMenu();
        for (int i = 0; i < categories.Length; i++)
        {
            int index = i;
            GameObject button = ScrollView_Categories.AddUI(categories[i].categoryName, CategoryButtonUI);

            Text[] texts = button.GetComponentsInChildren<Text>();

            texts[0].text = categories[i].categoryName;
            int clearedMaps = 0;
            for (int j = 0; j < categories[i].maps.Length; j++)
            {
                if (FileManager.IsMapCleared(categories[i].maps[j].mapName))
                {
                    clearedMaps++;
                }
            }
            float percentage = (float)clearedMaps / categories[i].maps.Length;
            if (float.IsNaN(percentage))
            {
                percentage = 1f;
            }
            texts[1].text = (percentage * 100).ToString("N2") + "% Cleared";
            texts[1].color = new Color((1-percentage) * 255, percentage * 255, 0f);
            button.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(() => OnCategory(index)));
        }
        Public.soundManager.SetBGM(bgm);
    }

    public void OnCategory(int _index)
    {
        ScrollView_Maps.ResetMenu();
        currentCategory = categories[_index];
        for (int i = 0; i < currentCategory.maps.Length; i++)
        {
            int index = i;
            GameObject button = ScrollView_Maps.AddUI(currentCategory.maps[i].mapName, MapButtonUI);
            Text[] texts = button.GetComponentsInChildren<Text>();
            texts[0].text = currentCategory.maps[i].mapName;
            texts[1].text = currentCategory.maps[i].difficulty.ToString();
            Image image = button.GetComponentInChildren<Image>();
            if (FileManager.IsMapCleared(currentCategory.maps[i].mapName))
            {
                image.color = Color.green;
            }
            else
            {
                image.color = Color.red;
            }
            button.GetComponent<Button>().onClick.AddListener(new UnityAction(() => OnMap(index)));
        }
        StartScrollAnimation();
    }

    public void OnMap(int _index)
    {
        isMapSelected = true;
        Public.selectedMap = currentCategory.maps[_index];
    }

    public void Update()
    {
        if (isMapSelected)
        {
            TextUI_Start.text = "Start " + Public.selectedMap.mapName;
        }
        else
        {
            TextUI_Start.text = "Non-Selected";
        }
    }

    public void OnStart()
    {
        if (isMapSelected)
        {
            SceneManager.LoadScene(SceneNames.GameScene);
        }
    }

    public void OnExit()
    {
        SceneManager.LoadScene(SceneNames.MainScene);
    }

    private IEnumerator ScrollAnimationI;

    private void StartScrollAnimation()
    {
        try
        {
            StopCoroutine(ScrollAnimationI);
        }
        catch
        {

        }
        ScrollAnimationI = ScrollAnimation();
        StartCoroutine(ScrollAnimationI);
    }

    private IEnumerator ScrollAnimation()
    {
        float y = -1000;
        while (true)
        {
            y = Mathf.Lerp(y, 0f, Time.deltaTime * animationSpeed);
            ScrollAnimationHolder.anchoredPosition = new Vector2(0f, y);
            if(y > -1f)
            {
                ScrollAnimationHolder.anchoredPosition = Vector2.zero;
                yield break;
            }
            yield return null;
        }
    }
}
