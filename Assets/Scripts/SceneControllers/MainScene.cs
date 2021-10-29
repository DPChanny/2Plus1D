using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;

    private void Start()
    {
        Public.logManager.AddLog("Welcome");
        Public.soundManager.SetBGM(bgm);
        Public.init = true;
    }

    public void OnStart()
    {
        SceneManager.LoadScene(SceneNames.MapSelectionScene);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
