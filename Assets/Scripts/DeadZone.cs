using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnController respawnController = other.GetComponent<RespawnController>();
            if (respawnController != null)
            {
                respawnController.Die();
            }
        }
    }
}