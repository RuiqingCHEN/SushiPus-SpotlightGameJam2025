using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SelectableUnit : MonoBehaviour
{
    private NavMeshAgent Agent;
    [SerializeField] private SpriteRenderer SelectionSprite;

    [SerializeField] private float StopThreshold = 0.1f;

    private bool isInTriggerZone = false;
    private float timeStoppedInZone = 0f;
    private const float REQUIRED_STOP_TIME = 2f;

    private void Awake()
    {
        SelectionManager.Instance.AvailableUnits.Add(this);
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isInTriggerZone && Agent != null)
        {
            if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (Agent.velocity.magnitude < StopThreshold)
                {
                    timeStoppedInZone += Time.deltaTime;

                    if (timeStoppedInZone >= REQUIRED_STOP_TIME)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    timeStoppedInZone = 0f;
                }
            }
            else
            {
                timeStoppedInZone = 0f;
            }
        }
    }

    public void MoveTo(Vector3 Position)
    {
        if (Agent != null)
        {
            Agent.SetDestination(Position);
            timeStoppedInZone = 0f;
        }
    }

    public void EnterTriggerZone()
    {
        isInTriggerZone = true;
        timeStoppedInZone = 0f;
    }

    public void ExitTriggerZone()
    {
        isInTriggerZone = false;
        timeStoppedInZone = 0f;
    }

    public void OnSelected()
    {
        if (SelectionSprite != null)
            SelectionSprite.gameObject.SetActive(true);
    }

    public void OnDeselected()
    {
        if (SelectionSprite != null)
            SelectionSprite.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SelectionManager.Instance.AvailableUnits.Remove(this);
        SelectionManager.Instance.SelectedUnits.Remove(this);

        if (isInTriggerZone && timeStoppedInZone >= REQUIRED_STOP_TIME)
        {
            if (PlayerInventory.Instance != null)
            {
                PlayerInventory.Instance.CollectCoin();
            }
        }
    }
}