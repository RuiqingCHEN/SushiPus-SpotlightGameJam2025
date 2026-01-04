using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;
    [SerializeField] private float checkDistance = 2f;
    Collider checkpointCollider;

    private void Awake()
    {
        checkpointCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RespawnController respawnController = other.GetComponent<RespawnController>();
            GroundController groundController = other.GetComponent<GroundController>();
            if (respawnController != null && groundController != null)
            {
                LayerMask playerGroundMask = groundController.GetGroundLayerMask();
                if (Physics.Raycast(respawnPoint.position, Vector3.down, checkDistance, playerGroundMask))
                {
                    respawnController.UpdateCheckpoint(respawnPoint.position);
                    checkpointCollider.enabled = false;
                }
            }
        }
    }
}
