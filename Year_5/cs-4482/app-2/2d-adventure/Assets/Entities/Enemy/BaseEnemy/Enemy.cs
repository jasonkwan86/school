using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    
    private void Start()
    {
        hurtbox.OnHurt += HandleHurt;
    }
    
    private void HandleHurt()
    {
        Destroy(gameObject);
    }
}
