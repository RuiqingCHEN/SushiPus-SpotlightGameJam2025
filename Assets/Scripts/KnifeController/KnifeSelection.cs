using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using URPGlitch;

public class KnifeSelection : MonoBehaviour
{
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);  

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    private Transform highlight;
    private RaycastHit raycastHit;
    private bool isExplodeMode = false;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite enabledSprite;
    [SerializeField] private Sprite disabledSprite;

    [SerializeField] private Texture2D enabledCursor;

    [Header("Selection Settings")]
    [SerializeField] private LayerMask selectableLayerMask;

    [Header("Post Processing Settings")]
    [SerializeField] private Volume globalVolume;
    
    private AnalogGlitchVolume analogGlitchVolume;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out analogGlitchVolume);
        }
    }

    void Update()
    {
        if (!isExplodeMode) return;

        if (highlight != null)
        {
            RestoreColors();
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, 100f, selectableLayerMask))
        {
            highlight = raycastHit.transform;
            
            if (highlight.CompareTag("Player"))
            {
                PlayerReference playerRef = highlight.GetComponent<PlayerReference>();
                
                if (playerRef != null)
                {  
                    if (playerRef.splitController != null)
                    {
                        if (playerRef.splitController.CanSplit())
                        {
                            ApplyHighlight(highlight);
                        }
                        else
                        {
                            highlight = null;
                        }
                    }
                    else
                    {
                        highlight = null;
                    }
                }
                else
                {
                    highlight = null;
                }
            }
            else if (highlight.CompareTag("Selectable"))
            {
                ApplyHighlight(highlight);
            }
            else
            {
                highlight = null;
            }
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (highlight)
            {
                SoundManager.PlaySFX("KnifeClick");

                if (highlight.CompareTag("Player"))
                {
                    PlayerReference playerRef = highlight.GetComponent<PlayerReference>();
                    
                    if (playerRef != null && playerRef.splitController != null)
                    {
                        if (playerRef.splitController.CanSplit())
                        {
                            playerRef.splitController.TriggerSplit();
                        }
                    }
                }
                else if (highlight.CompareTag("Selectable"))
                {
                    Destructible destructible = highlight.GetComponent<Destructible>();
                    if (destructible != null)
                    {
                        destructible.Explode();
                    }
                    else
                    {
                        Destroy(highlight.gameObject);
                    }
                }

                RestoreColors();
                highlight = null;
            }
        }
    }
    
    private void ApplyHighlight(Transform target)
    {
        originalColors.Clear();
        
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer != null && renderer.material != null)
            {
                if (renderer.material.HasProperty("_BaseColor"))
                {
                    originalColors[renderer] = renderer.material.GetColor("_BaseColor");
                    renderer.material.SetColor("_BaseColor", highlightColor);
                }
                else if (renderer.material.HasProperty("_Color"))
                {
                    originalColors[renderer] = renderer.material.color;
                    renderer.material.color = highlightColor;
                }
            }
        }
        
        SkinnedMeshRenderer[] skinnedRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
        {
            if (renderer != null && renderer.material != null)
            {
                if (renderer.material.HasProperty("_BaseColor"))
                {
                    originalColors[renderer] = renderer.material.GetColor("_BaseColor");
                    renderer.material.SetColor("_BaseColor", highlightColor);
                }
                else if (renderer.material.HasProperty("_Color"))
                {
                    originalColors[renderer] = renderer.material.color;
                    renderer.material.color = highlightColor;
                }
            }
        }
    }

    private void RestoreColors()
    {
        foreach (var kvp in originalColors)
        {
            if (kvp.Key != null && kvp.Key.material != null)
            {
                if (kvp.Key.material.HasProperty("_BaseColor"))
                {
                    kvp.Key.material.SetColor("_BaseColor", kvp.Value);
                }
                else if (kvp.Key.material.HasProperty("_Color"))
                {
                    kvp.Key.material.color = kvp.Value;
                }
            }
        }
        originalColors.Clear();
    }
    
    public void ToggleSelectionMode()
    {
        isExplodeMode = !isExplodeMode;

        SoundManager.PlaySFX("KnifeAppear");

        if (buttonImage != null)
        {
            buttonImage.sprite = isExplodeMode ? enabledSprite : disabledSprite;
        }

        if (isExplodeMode)
        {
            Cursor.SetCursor(enabledCursor, new Vector2(139, 136), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
        }
        
        if (!isExplodeMode && highlight != null)
        {
            RestoreColors();
            highlight = null;
        }

        UpdateGlitchEffect();
    }

    private void UpdateGlitchEffect()
    {
        if (analogGlitchVolume != null)
        {
            analogGlitchVolume.colorDrift.overrideState = true;
            analogGlitchVolume.colorDrift.value = isExplodeMode ? 0.3f : 0f;
        }
    }
}