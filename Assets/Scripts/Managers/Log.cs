using UnityEngine;

//Log Controller for five second destroy

public class Log : MonoBehaviour
{
    [HideInInspector]
    public int id;

    private void Start()
    {
        Invoke("Destroy", 5f);
    }

    private void Destroy()
    {
        Public.logManager.RemoveLog(id);
    }
}
