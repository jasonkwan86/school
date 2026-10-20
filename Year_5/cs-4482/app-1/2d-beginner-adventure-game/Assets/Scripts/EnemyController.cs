using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    public bool vertical;
    public float changeTime = 3.0f;
    float _timer;
    int _direction = 1;
    Rigidbody2D _rigidbody2d;
    Animator animator;
    bool broken = true;
    
    private void Start()
    {
        _rigidbody2d =  GetComponent<Rigidbody2D>();
        _timer = changeTime;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _timer  -= Time.deltaTime;
        
        if (_timer < 0)
        {
            _direction = -_direction;
            _timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }
        Vector2 position = _rigidbody2d.position;
        
        if (vertical)
        {
            position.y = position.y + speed * _direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", _direction);
        }
        else
        {
            position.x = position.x + speed * _direction * Time.deltaTime;
            animator.SetFloat("Move X", _direction);
            animator.SetFloat("Move Y", 0);
        }
        _rigidbody2d.MovePosition(position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    
    public void Fix()
    {
        broken = false;
        _rigidbody2d.simulated = false;
    }
}
