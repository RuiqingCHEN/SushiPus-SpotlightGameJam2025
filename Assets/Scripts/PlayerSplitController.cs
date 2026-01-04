using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerSplitController : MonoBehaviour
{
    public GameObject mergedPlayer;
    public GameObject splitPlayer1;
    public GameObject splitPlayer2;
    
    public float splitDistance = 2f;
    public float mergeDistance = 1.5f;
    public float animationDuration = 0.3f;
    
    public CinemachineCamera virtualCamera;
    public ParticleSystem m_ParticleSystem;
    
    private bool isSplit = false;
    private GameObject activePlayer;

    void Start()
    {
        mergedPlayer.SetActive(true);
        splitPlayer1.SetActive(false);
        splitPlayer2.SetActive(false);
        activePlayer = mergedPlayer;
        
        if (virtualCamera != null)
            virtualCamera.Follow = activePlayer.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isSplit)
        {
            Switch();
        }

        if (Input.GetKeyDown(KeyCode.C) && isSplit)
        {
            float distance = Vector3.Distance(splitPlayer1.transform.position, splitPlayer2.transform.position);
            if (distance <= mergeDistance)
            {
                StartCoroutine(Merge());
            }
        }
    }
    
    public bool CanSplit()
    {
        return !isSplit;
    }
    
    public void TriggerSplit()
    {
        if (!isSplit)
        {
            StartCoroutine(Split());
        }
    }

    IEnumerator Split()
    {
        Vector3 centerPos = mergedPlayer.transform.position;
        Vector3 splitDir = Vector3.up;
        
        Vector3 targetPos1 = centerPos + splitDir * splitDistance; 
        Vector3 targetPos2 = centerPos;
        
        splitPlayer1.transform.position = centerPos + Vector3.up * 0.8f;
        splitPlayer2.transform.position = centerPos;
        
        splitPlayer1.SetActive(true);
        splitPlayer2.SetActive(true);
        mergedPlayer.SetActive(false);
        
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            
            splitPlayer1.transform.position = Vector3.Lerp(centerPos, targetPos1, t);
            splitPlayer2.transform.position = Vector3.Lerp(centerPos, targetPos2, t);
            
            yield return null;
        }
        
        isSplit = true;
        activePlayer = splitPlayer1;

        activePlayer.GetComponent<PlayerController>().enabled = true;
        splitPlayer2.GetComponent<PlayerController>().enabled = false;

        SoundManager.PlaySFX("Separate");
        
        if (virtualCamera != null)
            virtualCamera.Follow = activePlayer.transform;
            
        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.transform.position = activePlayer.transform.position;
            m_ParticleSystem.Play();
        }
    }

    void Switch()
    {
        activePlayer.GetComponent<PlayerController>().enabled = false;
        
        activePlayer = (activePlayer == splitPlayer1) ? splitPlayer2 : splitPlayer1;

        activePlayer.GetComponent<PlayerController>().enabled = true;
        
        SoundManager.PlaySFX("Change");
        
        if (virtualCamera != null)
            virtualCamera.Follow = activePlayer.transform;
            
        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.transform.position = activePlayer.transform.position;
            m_ParticleSystem.Play();
        }
    }

    IEnumerator Merge()
    {
        splitPlayer1.GetComponent<PlayerController>().enabled = false;
        splitPlayer2.GetComponent<PlayerController>().enabled = false;

        Vector3 midPoint = (splitPlayer1.transform.position + splitPlayer2.transform.position) / 2f;
        Vector3 startPos1 = splitPlayer1.transform.position;
        Vector3 startPos2 = splitPlayer2.transform.position;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;

            splitPlayer1.transform.position = Vector3.Lerp(startPos1, midPoint, t);
            splitPlayer2.transform.position = Vector3.Lerp(startPos2, midPoint, t);

            yield return null;
        }

        splitPlayer1.SetActive(false);
        splitPlayer2.SetActive(false);

        mergedPlayer.transform.position = midPoint;
        mergedPlayer.SetActive(true);

        isSplit = false;
        activePlayer = mergedPlayer;

        activePlayer.GetComponent<PlayerController>().enabled = true;

        SoundManager.PlaySFX("Combine");

        if (virtualCamera != null)
            virtualCamera.Follow = activePlayer.transform;

        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.transform.position = activePlayer.transform.position;
            m_ParticleSystem.Play();
        }
    }
    
    public Transform GetActivePlayerTransform()
    {
        return activePlayer != null ? activePlayer.transform : null;
    }
}