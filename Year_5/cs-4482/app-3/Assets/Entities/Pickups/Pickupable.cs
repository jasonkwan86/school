using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private Pickup pickupEffect;
    [SerializeField] private AudioClip audioClip;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null) return;
        
        // Perform effect here, health pickup/give player auto attacker
        pickupEffect.ApplyEffect(player);
        FindObjectOfType<AudioSource>().PlayOneShot(audioClip);
        Destroy(gameObject);
    }
}
