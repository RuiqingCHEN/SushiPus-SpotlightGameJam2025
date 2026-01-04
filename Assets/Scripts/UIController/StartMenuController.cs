using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [Header("Start Button Settings")]
    public Button startButton; 
    public Sprite[] buttonIcons;
    public Image buttonIconImage;

    [Header("Background Animation Settings")]
    public Animator backgroundAnimator; 

    [Header("Title Animation Settings")]
    public Animator titleAnimator; 

    private int clickCount = 0; 
    private bool isPlayingAnimation = false;

    void Start()
    {
        if (buttonIcons.Length > 0 && buttonIconImage != null)
        {
            buttonIconImage.sprite = buttonIcons[0];
        }
    }

    public void OnStartClick()
    {
        if (isPlayingAnimation)
            return;

        if (clickCount == buttonIcons.Length - 2)
        {
            StartCoroutine(PlayBackgroundAnimation());
            return;
        }

        if (clickCount >= buttonIcons.Length - 1)
        {
            GetComponent<ASyncManager>().LoadBtn();
            return;
        }

        clickCount++;

        if (clickCount < buttonIcons.Length && buttonIconImage != null)
        {
            buttonIconImage.sprite = buttonIcons[clickCount];
        }
    }
    
    private IEnumerator PlayBackgroundAnimation()
    {
        isPlayingAnimation = true;

        if (backgroundAnimator != null)
        {
            backgroundAnimator.CrossFadeInFixedTime("Glitch", 0f);

            yield return new WaitForSeconds(2.16f);

            backgroundAnimator.CrossFadeInFixedTime("Normal2", 0f);
        }
        
        if (titleAnimator != null)
        {
            titleAnimator.CrossFadeInFixedTime("titleGlitch", 0f);
        }

        clickCount++;
        
        if (clickCount < buttonIcons.Length && buttonIconImage != null)
        {
            buttonIconImage.sprite = buttonIcons[clickCount];
        }
        
        isPlayingAnimation = false;
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}