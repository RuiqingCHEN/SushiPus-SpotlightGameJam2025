using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public int NumberOfCoins { get; private set; }

    public event Action<int> OnCoinCollected;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else if(Instance != this) Destroy(this);
    }
    public void CollectCoin()
    {
        NumberOfCoins++;
        SoundManager.PlaySFX("CoinDrop");
        OnCoinCollected?.Invoke(NumberOfCoins);
    }
}