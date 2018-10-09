using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager sInstance;

    public static AudioManager Instance
    {
        get
        {
            return sInstance;
        }
    }

    private Dictionary<int, string> audioPathDict; // 存放音频文件路径

    private AudioSource musicAudioSource;

    private List<AudioSource> unusedSoundAudioSourceList; // 存放可以使用的音频组件

    private List<AudioSource> usedSoundAudioSourceList; // 存放正在使用的音频组件

    private Dictionary<int, AudioClip> audioClipDict; // 缓存音频文件

    private float musicVolume = 1.0f;

    private float soundVolume = 1.0f;

    private string PREFS_MUSIC_VOLUME = "MusicVolume";

    private string PREFS_SOUND_VOLUME = "SoundVolume";

    private int poolCount = 16; // 对象池数量

    public float MusicVolume { get { return musicVolume; } }
    public float SoundVolume { get { return soundVolume; } }

    private AudioManager() {}

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        sInstance = this;

        audioPathDict = new Dictionary<int, string>() // 这里设置音频文件路径。需要修改。 TODO
        {
            { (int) AudioConstant.BG_CLASSICAL, "Audio/BGM/bg_classical" },
            { (int) AudioConstant.SKILL_GET, "Audio/SFX/Skill_Get" },
            { (int) AudioConstant.LIFE_GET, "Audio/SFX/Life_Get" },
            { (int) AudioConstant.START, "Audio/SFX/Start" },
            { (int) AudioConstant.SHOOT01, "Audio/SFX/Shoot01" },
            { (int) AudioConstant.HIT01, "Audio/SFX/Hit01" },
            { (int) AudioConstant.ENEMY_DESTORY01, "Audio/SFX/Enemy_Destory01" },
            { (int) AudioConstant.SKILL01, "Audio/SFX/Skill01" },
            { (int) AudioConstant.SKILL02, "Audio/SFX/Skill02" },
            { (int) AudioConstant.GET01, "Audio/SFX/Get01" },
        };

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        unusedSoundAudioSourceList = new List<AudioSource>();
        usedSoundAudioSourceList = new List<AudioSource>();
        audioClipDict = new Dictionary<int, AudioClip>();
    }

    void Start()
    {
        // 从本地缓存读取声音音量
        if (PlayerPrefs.HasKey(PREFS_MUSIC_VOLUME))
        {
            musicVolume = PlayerPrefs.GetFloat(PREFS_MUSIC_VOLUME);
        }
        if (PlayerPrefs.HasKey(PREFS_SOUND_VOLUME))
        {
            soundVolume = PlayerPrefs.GetFloat(PREFS_SOUND_VOLUME);
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isLoop"></param>
    public void PlayMusic(int id, bool isLoop = true, float fadeTime = 0.5f)
    {
        // 通过DOTween将声音淡入淡出
        DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, 0, fadeTime).OnComplete(() =>
        {
            musicAudioSource.clip = GetAudioClip(id);
            musicAudioSource.clip.LoadAudioData();
            musicAudioSource.loop = isLoop;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.Play();
            DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, musicVolume, fadeTime);
        });
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="id"></param>
    public void PlaySound(int id, Action action = null)
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(id);
            audioSource.clip.LoadAudioData();
            audioSource.Play();

            StartCoroutine(WaitPlayEnd(audioSource, action));
        }
        else
        {
            AddAudioSource();

            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(id);
            audioSource.clip.LoadAudioData();
            audioSource.volume = soundVolume;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(WaitPlayEnd(audioSource, action));
        }
    }

    /// <summary>
    /// 播放3d音效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void Play3dSound(int id, Vector3 position)
    {
        AudioClip ac = GetAudioClip(id);
        AudioSource.PlayClipAtPoint(ac, position);
    }

    /// <summary>
    /// 当播放音效结束后，将其移至未使用集合
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    IEnumerator WaitPlayEnd(AudioSource audioSource, Action action)
    {
        yield return new WaitUntil(() =>
        {
            return !audioSource.isPlaying;
        });
        UsedToUnused(audioSource);
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// 获取音频文件，获取后会缓存一份
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(int id)
    {
        if (!audioClipDict.ContainsKey(id))
        {
            if (!audioPathDict.ContainsKey(id))
                return null;
            AudioClip ac = Resources.Load(audioPathDict[id]) as AudioClip;
            audioClipDict.Add(id, ac);
        }
        return audioClipDict[id];
    }

    /// <summary>
    /// 添加音频组件
    /// </summary>
    /// <returns></returns>
    private AudioSource AddAudioSource()
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            return UnusedToUsed();
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            unusedSoundAudioSourceList.Add(audioSource);
            return audioSource;
        }
    }

    /// <summary>
    /// 将未使用的音频组件移至已使用集合里
    /// </summary>
    /// <returns></returns>
    private AudioSource UnusedToUsed()
    {
        AudioSource audioSource = unusedSoundAudioSourceList[0];
        unusedSoundAudioSourceList.RemoveAt(0);
        usedSoundAudioSourceList.Add(audioSource);
        return audioSource;
    }

    /// <summary>
    /// 将使用完的音频组件移至未使用集合里
    /// </summary>
    /// <param name="audioSource"></param>
    private void UsedToUnused(AudioSource audioSource)
    {
        if (usedSoundAudioSourceList.Contains(audioSource))
        {
            usedSoundAudioSourceList.Remove(audioSource);
        }
        if (unusedSoundAudioSourceList.Count >= poolCount)
        {
            Destroy(audioSource);
        }
        else if (audioSource != null && !unusedSoundAudioSourceList.Contains(audioSource))
        {
            unusedSoundAudioSourceList.Add(audioSource);
        }
    }

    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;

        PlayerPrefs.SetFloat(PREFS_MUSIC_VOLUME, volume);
    }

    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        for (int i = 0; i < unusedSoundAudioSourceList.Count; i++)
        {
            unusedSoundAudioSourceList[i].volume = volume;
        }
        for (int i = 0; i < usedSoundAudioSourceList.Count; i++)
        {
            usedSoundAudioSourceList[i].volume = volume;
        }

        PlayerPrefs.SetFloat(PREFS_SOUND_VOLUME, volume);
    }
}