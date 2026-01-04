using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    [Header("音效列表")]
    [SerializeField] private SoundClip[] soundClips;
    
    [Header("背景音乐列表")]
    [SerializeField] private AudioClip[] backgroundMusic;

    private Dictionary<string, AudioClip> soundDict;

    private void Awake()
    {
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in soundClips)
        {
            soundDict[sound.name] = sound.clip;
        }
    }

    public AudioClip GetClip(string soundName)
    {
        if (soundDict.ContainsKey(soundName))
        {
            return soundDict[soundName];
        }
        Debug.LogWarning($"音效 '{soundName}' 不存在");
        return null;
    }

    public AudioClip GetBackgroundMusic(int index)
    {
        if (index >= 0 && index < backgroundMusic.Length)
            return backgroundMusic[index];
        return null;
    }
}

[System.Serializable]
public struct SoundClip
{
    public string name;      // 音效名称，如 "Footstep", "Jump", "UIClick"
    public AudioClip clip;   // 音效文件
}