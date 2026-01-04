using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private string soundName = "GeneralClick";
    
    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }
    
    private void PlaySound()
    {
        SoundManager.PlaySFX(soundName);
    }
}