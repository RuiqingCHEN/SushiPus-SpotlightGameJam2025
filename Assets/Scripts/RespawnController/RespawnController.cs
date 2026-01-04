using System.Collections;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    GroundController groundController;
    public bool IsDead { get; private set; } = false;

    Vector3 checkpointPos;
    Rigidbody rb;
    Collider[] colliders;

    private float _invincibilityDuration = 0.5f;
    private float _invincibilityEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>();
        groundController = GetComponent<GroundController>();
    }

    private void Start()
    {
        checkpointPos = transform.position  + Vector3.up;
    }

    public void Die()
    {
        if (IsDead  || Time.time < _invincibilityEndTime) return;

        IsDead = true;
        Debug.Log("玩家死了！");
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.localScale = Vector3.zero;

        if (groundController) groundController.enabled = false;
        foreach (Collider col in colliders)
            col.enabled = false;

        yield return new WaitForSeconds(duration);

        transform.position = checkpointPos;
        transform.rotation = Quaternion.identity;

        yield return new WaitForFixedUpdate();

        _invincibilityEndTime = Time.time + _invincibilityDuration;
        IsDead = false;

        transform.localScale = Vector3.one;

        foreach (Collider col in colliders)
            col.enabled = true;

        rb.isKinematic = false;

        yield return new WaitForFixedUpdate();
        if (groundController) groundController.enabled = true;
    }
    
    public void UpdateCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
    }
}