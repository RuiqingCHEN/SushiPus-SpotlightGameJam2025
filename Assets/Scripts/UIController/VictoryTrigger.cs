using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
[SerializeField] private GameObject victoryPanel;
    
    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowVictory();
        }
    }
    
    private void ShowVictory()
    {
        PauseController.SetPause(true);
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }
}
