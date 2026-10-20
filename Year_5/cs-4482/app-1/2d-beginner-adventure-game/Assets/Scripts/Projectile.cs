using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        Debug.Log("Projectile collision with " + other.gameObject);
        if (enemy != null)
        {
            enemy.Fix();
        }
        Destroy(gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
