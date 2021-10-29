using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerUIController : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private GameObject textView;

    private float startEndSpace;
    private float pieceSize;
    private readonly float areaSize = 600f;

    [SerializeField]
    private Image before;
    [SerializeField]
    private Image next;

    public Dictionary<string, RectTransform> rectTransforms = new Dictionary<string, RectTransform>();
    [HideInInspector]
    public List<string> keys = new List<string>();

    public GameObject AddUI(string _key, GameObject _UI)
    {
        RectTransform newUI = Instantiate(_UI, scrollRect.content).GetComponent<RectTransform>();
        rectTransforms.Add(_key, newUI);
        keys.Add(_key);
        InitView();
        return newUI.gameObject;
    }

    private void Start()
    {
        pieceSize = textView.GetComponent<RectTransform>().sizeDelta.x;
        startEndSpace = (areaSize / 2) - (pieceSize/2);

        int count = Public.currentMap.GetCountOfLayer();

        ResetMenu();

        for (int i = 0; i < count; i++)
        {
            AddUI(i.ToString(), textView).GetComponentInChildren<Text>().text = (i + 1).ToString();
        }

        SetArrow();

        float targetValue;

        if (Public.currentMap.currentLayerIndex == 0)
        {
            targetValue = 0f;
        }
        else if (Public.currentMap.currentLayerIndex == Public.currentMap.GetCountOfLayer() - 1)
        {
            targetValue = 1f;
        }
        else
        {
            targetValue = (startEndSpace + pieceSize * Public.currentMap.currentLayerIndex + pieceSize / 2) / scrollRect.content.sizeDelta.x;

        }
        scrollbar.value = targetValue;

        for (int i = 0; i < keys.Count; i++)
        {
            if (i > Public.currentMap.currentLayerIndex + 1 || i < Public.currentMap.currentLayerIndex - 1)
            {
                rectTransforms[keys[i]].gameObject.SetActive(false);
            }
            else
            {
                rectTransforms[keys[i]].gameObject.SetActive(true);
            }
        }
    }

    private readonly float smoothAnimationSpeed = 5f;

    private IEnumerator SmoothAnimationI;

    public void OnWarp()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (i > Public.currentMap.currentLayerIndex + 1 || i < Public.currentMap.currentLayerIndex - 1)
            {
                rectTransforms[keys[i]].gameObject.SetActive(false);
            }
            else
            {
                rectTransforms[keys[i]].gameObject.SetActive(true);
            }
        }
        StartSmoothAnimation();
        SetArrow();
    }

    private void StartSmoothAnimation()
    {
        try
        {
            StopCoroutine(SmoothAnimationI);
        }
        catch
        {

        }
        SmoothAnimationI = SmoothAnimation();
        StartCoroutine(SmoothAnimationI);
    }

    private IEnumerator SmoothAnimation()
    {
        while (true)
        {
            float targetValue;
            if (Public.currentMap.currentLayerIndex == 0)
            {
                targetValue = 0f;
            }
            else if (Public.currentMap.currentLayerIndex == Public.currentMap.GetCountOfLayer() - 1)
            {
                targetValue = 1f;
            }
            else
            {
                targetValue = (startEndSpace + pieceSize * Public.currentMap.currentLayerIndex + pieceSize / 2) / scrollRect.content.sizeDelta.x;
            
            }

            if(Mathf.Abs(scrollbar.value - targetValue) < 0.01f)
            {
                scrollbar.value = targetValue;
                yield break;
            }
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetValue, Time.deltaTime * smoothAnimationSpeed);

            yield return null;
        }
    }

    private void InitView()
    {
        float x = startEndSpace;

        for (int i = 0; i < keys.Count; i++)
        {
            rectTransforms[keys[i]].anchoredPosition = new Vector2(x, 0f);
            x += rectTransforms[keys[i]].sizeDelta.x;
        }

        scrollRect.content.sizeDelta = new Vector2(x + startEndSpace, scrollRect.content.sizeDelta.y);
    }

    public void ResetMenu()
    {
        Transform[] childList = scrollRect.content.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
        keys = new List<string>();
        rectTransforms = new Dictionary<string, RectTransform>();
    }

    private void SetArrow()
    {
        if (Public.currentMap.GetLayer().NextLayerWarp)
        {
            next.color = Color.blue;
        }
        else
        {
            next.color = Color.red;
        }
        if (Public.currentMap.GetLayer().BeforeLayerWarp)
        {
            before.color = Color.blue;
        }
        else
        {
            before.color = Color.red;
        }
    }
}
