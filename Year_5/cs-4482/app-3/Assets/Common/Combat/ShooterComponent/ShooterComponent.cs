using UnityEngine;

public class ShooterComponent : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    
    public void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.SetDirection(transform.right);
    }
}
