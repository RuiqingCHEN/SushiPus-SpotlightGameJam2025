using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageSwitcher : MonoBehaviour
{
    [Header("图片设置")]
    [SerializeField] private Sprite[] images;
    [SerializeField] private Image displayImage;

    [Header("界面设置")]
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private Button clickButton; 

    [Header("动画设置")]
    [SerializeField] private Animator animator; 
    [SerializeField] private float animationDuration = 3f;

    private int currentImageIndex = 0;
    private bool isPlayingAnimation = false;
    
    private void Start()
    {
        if (clickButton != null)
        {
            clickButton.onClick.AddListener(NextImage);
        }

        if (animator != null)
        {
            animator.gameObject.SetActive(false);
        }

        ShowImagePanel();
    }
    
    public void ShowImagePanel()
    {
        if (imagePanel != null)
        {
            imagePanel.SetActive(true);
        }
        
        PauseController.SetPause(true);

        currentImageIndex = 0;
        isPlayingAnimation = false;
        UpdateImage();
    }
    
    public void NextImage()
    {
        if (isPlayingAnimation) return;

        if (images == null || images.Length == 0) return;
        
        currentImageIndex++;
        
        if (currentImageIndex >= images.Length)
        {
            PlayAnimationAndClose();
            return;
        }
        
        UpdateImage();
    }

    private void UpdateImage()
    {
        if (displayImage != null && images != null && currentImageIndex < images.Length)
        {
            displayImage.sprite = images[currentImageIndex];
        }
    }

    private void PlayAnimationAndClose()
    {
        isPlayingAnimation = true;

        if (displayImage != null)
        {
            displayImage.enabled = false;
        }

        if (animator != null)
        {
            animator.gameObject.SetActive(true);
            animator.Play("NewPlayer");
        }

        StartCoroutine(WaitForAnimationAndClose());
    }
    
    private System.Collections.IEnumerator WaitForAnimationAndClose()
    {
        yield return new WaitForSecondsRealtime(animationDuration);
        
        CloseImagePanel();
    }
    
    public void CloseImagePanel()
    {
        if (imagePanel != null)
        {
            imagePanel.SetActive(false);
        }

        if (displayImage != null)
        {
            displayImage.enabled = true;
        }

        if (animator != null)
        {
            animator.gameObject.SetActive(false);
        }

        PauseController.SetPause(false);
        isPlayingAnimation = false;
    }
    
    private void Update()
    {
        if (imagePanel != null && imagePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                NextImage();
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopAllCoroutines();
                CloseImagePanel();
            }
        }
    }
}
