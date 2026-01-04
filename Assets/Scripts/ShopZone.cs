using UnityEngine;

public class ShopZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SelectableUnit>(out SelectableUnit unit))
        {
            unit.EnterTriggerZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SelectableUnit>(out SelectableUnit unit))
        {
            unit.ExitTriggerZone();
        }
    }
}
