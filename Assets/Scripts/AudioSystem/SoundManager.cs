using UnityEngine;

/// <summary>
/// 极简音效管理器
/// - 播放一次性音效（走路、UI交互等）
/// - 管理循环背景音乐
/// - 管理循环音效（倒计时、警报等）
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private static AudioSource sfxSource;   // 音效
    private static AudioSource bgmSource;   // 背景音乐
    private static AudioSource loopSource;  // 循环音效
    private static SoundLibrary library;

    [Header("音量")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 0.7f;
    [Range(0f, 1f)] public float loopVolume = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            // 获取或创建 AudioSource
            AudioSource[] audioSources = GetComponents<AudioSource>();
            
            if (audioSources.Length >= 2)
            {
                sfxSource = audioSources[0];
                bgmSource = audioSources[1];
            }
            
            // 添加第三个 AudioSource 用于循环音效
            loopSource = gameObject.AddComponent<AudioSource>();
            library = GetComponent<SoundLibrary>();

            Setup();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBGM(0);
    }

    private void Setup()
    {
        sfxSource.spatialBlend = 0f;
        bgmSource.spatialBlend = 0f;
        bgmSource.loop = true;
        sfxSource.volume = sfxVolume;
        bgmSource.volume = bgmVolume;
        
        // 设置循环音效源
        loopSource.spatialBlend = 0f;
        loopSource.loop = true;
        loopSource.volume = loopVolume;
        loopSource.playOnAwake = false;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public static void PlaySFX(string soundName)
    {
        if (library == null) return;
        
        AudioClip clip = library.GetClip(soundName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public static void PlayBGM(int index)
    {
        if (library == null) return;
        
        AudioClip bgm = library.GetBackgroundMusic(index);
        if (bgm != null && bgmSource.clip != bgm)
        {
            bgmSource.clip = bgm;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public static void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    /// <summary>
    /// 播放循环音效（如倒计时警报）
    /// </summary>
    public static void PlayLoopSFX(string soundName)
    {
        if (library == null || loopSource == null) return;
        
        AudioClip clip = library.GetClip(soundName);
        if (clip != null)
        {
            // 如果正在播放相同音效，不重复播放
            if (loopSource.isPlaying && loopSource.clip == clip)
                return;
                
            loopSource.clip = clip;
            loopSource.Play();
        }
    }

    /// <summary>
    /// 停止循环音效
    /// </summary>
    public static void StopLoopSFX()
    {
        if (loopSource != null)
        {
            loopSource.Stop();
            loopSource.clip = null;
        }
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    public static void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    public static void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// 设置循环音效音量
    /// </summary>
    public static void SetLoopVolume(float volume)
    {
        if (loopSource != null)
        {
            loopSource.volume = Mathf.Clamp01(volume);
        }
    }
}