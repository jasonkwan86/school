using UnityEngine;

public class LongRangeAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    
    public void Execute(Vector3 direction)
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(direction));
        projectile.SetDirection(direction);
        projectile.gameObject.layer = gameObject.layer;
    }
}
