using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private SpriteRenderer playerTransform;
    
    private Vector2 _movement;

    private void Start()
    {
        moveAction.Enable();
    }

    private void Update()
    {
        _movement = moveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(_movement.x, 0.0f))
        {
            playerTransform.flipX = _movement.x < 0;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = player.position + moveSpeed * Time.deltaTime * _movement;
        player.MovePosition(position);
    }
}
