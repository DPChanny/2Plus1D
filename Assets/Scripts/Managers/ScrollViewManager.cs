using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스크롤뷰 매니저
public class ScrollViewManager : MonoBehaviour
{
    [SerializeField]
    private float space;

    //스크롤뷰
    [SerializeField]
    private ScrollRect scrollRect;

    //딕셔너리
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

    //스크롤뷰 초기화
    private void InitView()
    {
        float y = 0f;
        for (int i = 0; i < keys.Count; i++)
        {
            rectTransforms[keys[i]].anchoredPosition = new Vector2(0f, -y);
            y += rectTransforms[keys[i]].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    }

    //메뉴 초기화
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
}
