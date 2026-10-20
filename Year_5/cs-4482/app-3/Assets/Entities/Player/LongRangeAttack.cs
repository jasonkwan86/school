using UnityEngine;

public class LongRangeAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private AudioClip shootAudioClip;
    
    public void Execute(Vector3 direction)
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(direction));
        projectile.SetDirection(direction);
        projectile.gameObject.layer = LayerMask.NameToLayer("Player Projectile");
        projectile.SetColor(Color.cyan);
        FindObjectOfType<AudioSource>().PlayOneShot(shootAudioClip);
    }
}
