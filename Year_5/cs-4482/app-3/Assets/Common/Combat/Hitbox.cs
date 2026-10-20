using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        
        if (gameObject.layer == other.gameObject.layer)
        {
            return;
        }
        
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox)
        {
            hurtbox.EvokeOnHurt();
        }
    }
}
