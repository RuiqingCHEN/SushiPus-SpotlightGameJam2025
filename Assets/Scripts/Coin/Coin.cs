using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool collected = false;
    void OnTriggerEnter(Collider other)
    {
        if(collected) return;
        if(other.CompareTag("Player"))
        {
            collected = true;
            PlayerInventory.Instance.CollectCoin();
            gameObject.SetActive(false);
        }
    }
}