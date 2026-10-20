using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject linkedDoor;
    [SerializeField] private AudioClip audioClip;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Player>()) return;
        
        FindAnyObjectByType<AudioSource>().PlayOneShot(audioClip);
        Destroy(linkedDoor);
        Destroy(gameObject);
    }
}
