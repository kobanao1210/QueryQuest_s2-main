using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    private readonly Dictionary<string,AudioClip> BGMList = new Dictionary<string,AudioClip>();
    private readonly Dictionary<string, AudioClip> SEList = new Dictionary<string, AudioClip>();

    public enum EclipType
    {
        BGM,SE
    }

    public static AudioManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //外部からロード？
            DontDestroyOnLoad(gameObject);
            LoadAudioFile();
        }
        else Destroy(gameObject);
    }

    public void Play(string clipName,EclipType type)  //再生する音声によって、挙動変化（ループ,音かぶらせるかなど）
    {
        if(type == EclipType.BGM)
        {
            audioSources[0].clip = BGMList[clipName];
            audioSources[0].Play();
            audioSources[0].volume = 0.7f;
        }
        else audioSources[1].PlayOneShot(SEList[clipName]);
    }

    public void ToggleBGM()
    {
        if (audioSources[0].isPlaying) audioSources[0].Stop();
        else audioSources[0].Play();
    }

    public void ToggleBGM(float num)  //指定秒数(float)後にもとの状態に戻す
    {
        ToggleBGM();
        Invoke(nameof(ToggleBGM), num);
    }

    private void LoadAudioFile()
    {
        var bgmClips = Resources.LoadAll<AudioClip>("BGM");
        foreach (var clip in bgmClips) BGMList.Add(clip.name, clip);
        var seClips = Resources.LoadAll<AudioClip>("SE");
        foreach (var clip in seClips) SEList.Add(clip.name, clip);
    }
}
