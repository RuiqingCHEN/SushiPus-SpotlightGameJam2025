using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private PlayerInventory playerInventory;

    void OnEnable()
    {
        PlayerInventory.Instance.OnCoinCollected += UpdateCoinText;
        UpdateCoinText(PlayerInventory.Instance.NumberOfCoins);
    }

    void OnDisable()
    {
        PlayerInventory.Instance.OnCoinCollected -= UpdateCoinText;
    }

    private void UpdateCoinText(int coinCount)
    {
        coinText.text = "0" + coinCount.ToString();
    }
}