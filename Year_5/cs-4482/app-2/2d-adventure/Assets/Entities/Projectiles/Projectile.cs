using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 3f;
    
    private Vector3 _direction;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * speed * _direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.layer == other.gameObject.layer)
        {
            return;
        }

        if (!other.GetComponent<Hurtbox>())
        {
            return;
        }
        
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
}
