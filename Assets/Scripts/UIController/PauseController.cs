using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private RectTransform pausePanelRect;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private Animator pauseAnimator; 
    
    public static bool IsGamePaused { get; private set; } = false;
    
    private Vector2 originalPosition;
    
    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            
            if (pausePanelRect != null)
            {
                originalPosition = pausePanelRect.anchoredPosition;
            }
        }
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        if (IsGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0f;

        SoundManager.PlaySFX("MenuOpen");
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            
            if (pausePanelRect != null)
            {
                pausePanelRect.anchoredPosition = new Vector2(-Screen.width, originalPosition.y);
                
                // 从左向右滑入
                pausePanelRect.DOAnchorPos(originalPosition, 0.7f)
                    .SetEase(Ease.OutCubic)
                    .SetUpdate(true);
            }
        }
    }
    
    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;

        SoundManager.PlaySFX("MenuClose");
        
        if (pauseAnimator != null)
        {
            pauseAnimator.SetTrigger("Close");
        }
        
        if (pausePanelRect != null)
        {
            Vector2 targetPos = new Vector2(-Screen.width, originalPosition.y);
            
            pausePanelRect.DOAnchorPos(targetPos, 0.6f)
                .SetEase(Ease.InCubic)
                .SetUpdate(true);
        }
        
        StartCoroutine(CloseAfterAnimation());
    }
    
    private System.Collections.IEnumerator CloseAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            
            if (pausePanelRect != null)
            {
                pausePanelRect.anchoredPosition = originalPosition;
            }
        }
    }

    public void OnResumeClick()
    {
        ResumeGame();
    }

    public void OnMainMenuClick()
    {
        Time.timeScale = 1f;
        IsGamePaused = false;
        SceneManager.LoadScene(0);
    }
    
    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    static PauseController()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            IsGamePaused = false;
            Time.timeScale = 1f;
        };
    }

    public void OpenInstruction()
    {
        instructionPanel.SetActive(true);
    }
    
    public void CloseInstruction()
    {
        instructionPanel.SetActive(false);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}