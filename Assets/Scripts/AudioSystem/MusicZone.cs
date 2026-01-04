using UnityEngine;

/// <summary>
/// 音乐区域触发器 - 进入区域切换BGM
/// </summary>
[RequireComponent(typeof(Collider))]
public class MusicZone : MonoBehaviour
{
    [SerializeField] private int bgmIndex = 0;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool restoreOnExit = false;
    [SerializeField] private int defaultBgmIndex = 0;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SoundManager.PlayBGM(bgmIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (restoreOnExit && other.CompareTag(playerTag))
        {
            SoundManager.PlayBGM(defaultBgmIndex);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}