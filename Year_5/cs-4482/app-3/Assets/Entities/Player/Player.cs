using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hurtAudio;
    [SerializeField] private AudioClip healAudio;

    [SerializeField] private float invulnerabilityDuration = 0.5f;

    private float timeLeftUntilVulnerable;
    
    private void Start()
    {
        hurtbox.OnHurt += HandleHurt;
    }

    // Assume all damage is 1 health point
    private void HandleHurt()
    {
        if (timeLeftUntilVulnerable > 0) return;
        
        timeLeftUntilVulnerable = invulnerabilityDuration;
        healthComponent.TakeDamage(1);
        audioSource.time = 3f;
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void TakeHealthPickup(int healthToReplenish)
    {
        healthComponent.Heal(healthToReplenish);
    }

    private void Update()
    {
        timeLeftUntilVulnerable -= Time.deltaTime;
    }
}
