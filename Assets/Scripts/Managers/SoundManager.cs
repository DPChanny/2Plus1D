using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    private AudioSource bgm;

    private void Awake()
    {
        if (Public.init)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Public.soundManager = this;
        }
    }

    public void Play(AudioClip _audioClip)
    {
        sound.PlayOneShot(_audioClip);
    }

    public void SetBGM(AudioClip _audioClip)
    {
        if(bgm.clip != _audioClip)
        {
            bgm.clip = _audioClip;
            StartBGM();
        }
    }

    public void StartBGM()
    {
        bgm.loop = true;
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }
}
