using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private ShooterComponent shooterComponent;
    
    private void Start()
    {
        hurtbox.OnHurt += HandleHurt;
        
        InvokeRepeating(nameof(Shoot), 2f, 2f);
    }

    private void Shoot()
    {
        shooterComponent.Shoot();
    }
    
    private void HandleHurt()
    {
        Destroy(gameObject);
    }
}
