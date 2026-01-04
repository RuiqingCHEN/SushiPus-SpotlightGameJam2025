using UnityEngine;

/// <summary>
/// 3D空间音效发射器 - 物体上的循环音效
/// 用于：火焰声、水流声、机器声等
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool playOnStart = true;
    
    [Header("3D音效参数")]
    [Range(0f, 1f)] [SerializeField] private float volume = 1f;
    [Range(1f, 100f)] [SerializeField] private float maxDistance = 50f;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.spatialBlend = 1f;  // 完全3D
        source.maxDistance = maxDistance;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.playOnAwake = false;
    }

    private void Start()
    {
        if (playOnStart && clip != null) source.Play();
    }

    public void Play() => source.Play();
    public void Stop() => source.Stop();
    public void SetVolume(float vol) => source.volume = Mathf.Clamp01(vol);
}