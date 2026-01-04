using UnityEngine;

public class InfoTriggerZone : MonoBehaviour
{
    [Header("Popup Settings")]
    [Tooltip("要显示的info图片索引")]
    public int popupImageIndex = 0;
    
    [Tooltip("是否只触发一次")]
    public bool triggerOnce = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家
        if (other.CompareTag("Player"))
        {
            // 如果设置为只触发一次且已经触发过，则返回
            if (triggerOnce && hasTriggered)
                return;

            // 显示popup
            InfoPopupController.Instance.ShowInfo(popupImageIndex);
            
            hasTriggered = true;
        }
    }
}