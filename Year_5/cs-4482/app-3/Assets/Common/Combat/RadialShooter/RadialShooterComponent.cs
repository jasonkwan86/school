using UnityEngine;

public class RadialShooterComponent : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float timeBetweenShots = 0.5f;
    [SerializeField] private int spawnPointCount = 4;
    [SerializeField] private float rotationsPerSecond = 0.5f;

    private float degreesInternalRotation; 
    private float timeLeftUntilShoot = 0.75f;
    
    public void Shoot(Vector3 direction)
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.SetDirection(direction);
    }

    private void Update()
    {
        if (timeLeftUntilShoot <= 0)
        {
            float step = 2 * Mathf.PI / spawnPointCount;
            for (int i = 0; i < spawnPointCount; i++)
            {
                float currentAngle = step * i + degreesInternalRotation;
                Shoot(new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0));
            }
            timeLeftUntilShoot = timeBetweenShots;
        }
        else
        {
            timeLeftUntilShoot -= Time.deltaTime;
        }

        degreesInternalRotation += rotationsPerSecond * 2 * Mathf.PI * Time.deltaTime;
    }
}
