using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spawner : MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerDownHandler
{
    public GameObject prefab;
    public Transform spawnPoint;
    public PlayerSplitController playerSplitController;

    private GameObject spawnedObject;
    private Animator animator;
    private bool isSecondState = false;
    private bool isAnimating = false; 

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.Play("Normal1", 0, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSecondState && !isAnimating) 
        {
            animator.Play("Highlight1", 0, 0f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSecondState && !isAnimating) 
        {
            animator.Play("Normal1", 0, 0f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isAnimating = true; 
        
        if (!isSecondState)
        {
            animator.Play("Pressed1", 0, 0f);
            float pressed1Length = GetClipLength("Pressed1");
            Invoke("SwitchToNormal2", pressed1Length);
            isSecondState = true;
            
            SpawnObject();
        }
        else
        {
            animator.Play("Pressed2", 0, 0f);
            float pressed2Length = GetClipLength("Pressed2");
            Invoke("SwitchToNormal1", pressed2Length);
            isSecondState = false;
            
            SpawnObject();
        }
    }

    void SwitchToNormal2()
    {
        animator.Play("Normal2", 0, 0f);
        isAnimating = false; 
    }

    void SwitchToNormal1()
    {
        animator.Play("Normal1", 0, 0f);
        isAnimating = false;  
    }

    float GetClipLength(string clipName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        if (ac != null)
        {
            foreach (AnimationClip clip in ac.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
        }
        return 0.3f;
    }

    public void SpawnObject()
    {
        Vector3 spawnPosition = spawnPoint.position;
        Quaternion spawnRotation = spawnPoint.rotation;
        
        if (playerSplitController != null)
        {
            Transform activePlayerTransform = playerSplitController.GetActivePlayerTransform();
            if (activePlayerTransform != null)
            {
                spawnPosition = activePlayerTransform.position + Vector3.up * 6f;
                spawnRotation = activePlayerTransform.rotation;
            }
        }
        
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(prefab, spawnPosition, spawnRotation);
            SoundManager.PlaySFX("CoinAppear");
            SoundManager.PlaySFX("CoinDrop");
        }
        else
        {
            SoundManager.PlaySFX("CoinDisappear");
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}