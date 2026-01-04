using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;
public class CmcTransition : MonoBehaviour
{
[Header("Camera Settings")]
    public CinemachineCamera targetCamera;
    
    [Header("Camera Position 1")]
    public Vector3 offset1 = new Vector3(0, 6, -6);
    public Vector3 rotation1 = new Vector3(30, 0, 0);
    
    [Header("Camera Position 2")]
    public Vector3 offset2 = new Vector3(0, 6, 6);
    public Vector3 rotation2 = new Vector3(30, 90, 0);
    
    [Header("Transition Settings")]
    public float transitionDuration = 1f;
    public Ease easeType = Ease.InOutQuad;
    
    [Header("Player Displacement")]
    [SerializeField] private Vector3 displacement1to2 = new Vector3(0, 0, -1.5f);
    [SerializeField] private Vector3 displacement2to1 = new Vector3(0, 0, 1.5f); 
    
    private CinemachineFollow follow;
    private bool isInSetting2 = false;
    private bool canTrigger = true;

    void Start()
    {
        if (targetCamera != null)
        {
            follow = targetCamera.GetComponent<CinemachineFollow>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("Player") && targetCamera != null)
        {
            canTrigger = false;
            
            if (isInSetting2)
            {
                TransitionToSetting(offset1, rotation1);
                other.transform.position += displacement2to1;
                isInSetting2 = false;
            }
            else
            {
                TransitionToSetting(offset2, rotation2);
                other.transform.position += displacement1to2;
                isInSetting2 = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrigger = true;
        }
    }

    void TransitionToSetting(Vector3 targetOffset, Vector3 targetRotation)
    {
        DOTween.Kill(this); 
        
        if (follow != null)
        {
            DOTween.To(() => follow.FollowOffset, 
                    x => follow.FollowOffset = x, 
                    targetOffset, 
                    transitionDuration)
                .SetEase(easeType)
                .SetTarget(this); 
        }
        
        if (targetCamera != null)
        {
            targetCamera.transform.DORotate(targetRotation, transitionDuration)
                                .SetEase(easeType)
                                .SetTarget(this); 
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
