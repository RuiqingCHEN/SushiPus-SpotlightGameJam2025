using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    public float StartTime;
    private float TimeLeft;
    private bool isGameOver = false;

    public TMP_Text TimerText;

    [Header("Timer Visual Settings")]
    [SerializeField] private Image timerImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Animator timerAnimator;
    
    private bool hasChangedToWarning = false;
    private bool hasPlayedLastSecondAnim = false;

    void Start()
    {
        CountTimer();
        
        if (timerImage != null && normalSprite != null)
        {
            timerImage.sprite = normalSprite;
        }
    }

    void Update()
    {
        if (PauseController.IsGamePaused || isGameOver)
            return;
        
        if (TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            FormatToMinSec();

            // 进入警告阶段：播放循环音效
            if (TimeLeft < 31f && !hasChangedToWarning)
            {
                ChangeToWarningSprite();
                SoundManager.PlayLoopSFX("TimerLoop");  // ✅ 循环播放
                hasChangedToWarning = true;
            }

            // 最后一秒动画
            if (TimeLeft <= 1f && !hasPlayedLastSecondAnim)
            {
                PlayLastSecondAnimation();
            }
        }
        else
        {
            // 游戏结束
            if (!isGameOver)
            {
                isGameOver = true;
                TimeLeft = 0;
                FormatToMinSec();
                
                SoundManager.StopLoopSFX();         // ✅ 停止循环音效
                SoundManager.PlaySFX("TimerDing");  // ✅ 播放结束音效
                
                gameOverPanel.SetActive(true);
                PauseController.SetPause(true);
            }
        }
    }

    public void CountTimer()
    {
        TimeLeft = StartTime;
        isGameOver = false;
        hasChangedToWarning = false;
        hasPlayedLastSecondAnim = false;

        // 重置时停止循环音效
        SoundManager.StopLoopSFX();

        if (timerImage != null && normalSprite != null)
        {
            timerImage.sprite = normalSprite;
        }

        FormatToMinSec();
    }

    void FormatToMinSec()
    {
        float mins = Mathf.FloorToInt(TimeLeft / 60);
        float secs = Mathf.FloorToInt(TimeLeft % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }
    
    void ChangeToWarningSprite()
    {
        if (timerAnimator != null)
        {
            timerAnimator.Play("Warning");
        }
    }
    
    void PlayLastSecondAnimation()
    {
        if (timerAnimator != null)
        {
            timerAnimator.Play("LastSecond");
            hasPlayedLastSecondAnim = true;
        }
    }
}