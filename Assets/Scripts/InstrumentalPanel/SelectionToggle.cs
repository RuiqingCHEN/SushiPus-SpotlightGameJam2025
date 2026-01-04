using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using URPGlitch;

public class SelectionToggle : MonoBehaviour
{
    public static bool IsSelectionEnabled = false;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite enabledSprite;
    [SerializeField] private Sprite disabledSprite;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D enabledCursor;
    [SerializeField] private Texture2D clickCursor;

    [Header("Post Processing Settings")]
    [SerializeField] private Volume globalVolume;
    
    private AnalogGlitchVolume analogGlitchVolume;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out analogGlitchVolume);
        }
        
        UpdateButtonVisual();
    }

    private void Update()
    {
        if (IsSelectionEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.SetCursor(clickCursor, new Vector2(22, 13), CursorMode.Auto);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Cursor.SetCursor(enabledCursor, new Vector2(22, 13), CursorMode.Auto);
            }
        }
    }

    public void Toggle()
    {
        IsSelectionEnabled = !IsSelectionEnabled;
        SoundManager.PlaySFX("HandAppear"); 
        UpdateButtonVisual();
        UpdateGlitchEffect();
    }

    private void UpdateButtonVisual()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = IsSelectionEnabled ? enabledSprite : disabledSprite;
        }

        if (IsSelectionEnabled)
        {
            Cursor.SetCursor(enabledCursor, new Vector2(22, 13), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
        }
    }

    private void UpdateGlitchEffect()
    {
        if (analogGlitchVolume != null)
        {
            analogGlitchVolume.scanLineJitter.overrideState = true;
            analogGlitchVolume.scanLineJitter.value = IsSelectionEnabled ? 0.3f : 0f;
        }
    }
}