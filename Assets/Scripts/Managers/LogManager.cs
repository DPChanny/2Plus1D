using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//LogManager

public class LogManager : MonoBehaviour
{
    //Scroll Rect for Logs
    [SerializeField]
    private ScrollRect ScrollUI_Logs;

    //Log Prefab
    [SerializeField]
    private GameObject logPrefab;
   
    //List of logs
    private Dictionary<int, RectTransform> logs = new Dictionary<int, RectTransform>();

    //List of ids for logs
    private List<int> ids = new List<int>();

    private int count = 0;

    //Init
    private void Awake()
    {
        if (Public.init)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Public.logManager = this;
        }
    }

    //Add log
    public void AddLog(string log)
    {
        RectTransform newLog = Instantiate(logPrefab, ScrollUI_Logs.content).GetComponent<RectTransform>();
        newLog.GetComponent<Text>().text = log;
        newLog.GetComponent<Log>().id = count;
        ids.Add(count);
        logs.Add(count, newLog);
        UpdateLog();
        count++;
    }

    //Remove Log
    public void RemoveLog(int id)
    {
        Destroy(logs[id].gameObject);
        logs.Remove(id);
        ids.Remove(id);
        UpdateLog();
    }

    //Update Scroll Rect
    public void UpdateLog()
    {
        float y = 0f;
        for (int i = 0; i < ids.Count; i++)
        {
            logs[ids[i]].anchoredPosition = new Vector2(0f, y);
            y += logs[ids[i]].sizeDelta.y;
        }
    }
}
