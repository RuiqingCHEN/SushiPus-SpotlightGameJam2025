using UnityEngine;

public class SteppingStone : MonoBehaviour
{
 private Rigidbody rb;
    private float lastSoundTime;
    private float soundCooldown = 0.5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.relativeVelocity.magnitude > 0.5f)
            {
                PlayPushSound();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && 
            Time.time > lastSoundTime + soundCooldown)
        {
            if (rb.linearVelocity.magnitude > 0.3f)
            {
                PlayPushSound();
            }
        }
    }

    private void PlayPushSound()
    {
        SoundManager.PlaySFX("CoinRoll"); 
        lastSoundTime = Time.time;
    }
}
