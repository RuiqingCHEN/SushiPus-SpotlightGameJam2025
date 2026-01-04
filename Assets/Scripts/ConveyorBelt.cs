using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private Vector3 ConveyorDirection = Vector3.forward;
    [SerializeField] private float ConveyorSpeed = 2f;

    private List<Rigidbody> ObjectsOnBelt = new List<Rigidbody>();

    private void FixedUpdate()
    {
        ObjectsOnBelt.RemoveAll(rb => rb == null || !rb.gameObject.activeInHierarchy);

        foreach (var rb in ObjectsOnBelt)
        {
            if (rb != null)
            {
                Vector3 conveyorDisplacement = ConveyorDirection.normalized * ConveyorSpeed * Time.fixedDeltaTime;
                conveyorDisplacement.y = 0;
                
                rb.MovePosition(rb.position + conveyorDisplacement);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb) && !ObjectsOnBelt.Contains(rb))
        {
            ObjectsOnBelt.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            ObjectsOnBelt.Remove(rb);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}