using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private float speed = 2;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Player player;
    
    private void Start()
    {
        hurtbox.OnHurt += HandleHurt;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    
    private void HandleHurt()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            Vector3 direction = toPlayer.normalized;
            spriteRenderer.flipX = direction.x > 0;
            if (toPlayer.magnitude < 8)
            {
                transform.position += Time.deltaTime * speed * direction;
            }
        }
    }
}
