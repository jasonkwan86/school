using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private SpriteRenderer playerTransform;
    [SerializeField] private InputAction rollAction;
    [SerializeField] private float rollDuration = 0.5f;
    [SerializeField] private Animator playerMovementAnimator;

    private Vector2 _rollDirection;
    private Vector2 _movement;
    private float _rollTimeLeft;

    private void Start()
    {
        moveAction.Enable();
        rollAction.Enable();
    }

    private void Update()
    {
        _movement = moveAction.ReadValue<Vector2>();

        playerMovementAnimator.SetFloat("Move X", Mathf.Abs(_movement.magnitude));
        if (!Mathf.Approximately(_movement.x, 0.0f))
        {
            playerTransform.flipX = _movement.x < 0;
        }

        if (rollAction.ReadValue<float>() > 0 && _rollTimeLeft <= 0)
        {
            _rollTimeLeft = rollDuration;
            _rollDirection = _movement;
            playerMovementAnimator.SetBool("Sliding", true);
        }
    }

    private void FixedUpdate()
    {
        if (_rollTimeLeft > 0)
        {
            Vector2 position = player.position + moveSpeed * 2 * Time.fixedDeltaTime * _rollDirection;
            player.MovePosition(position);
            _rollTimeLeft -= Time.fixedDeltaTime;
        }
        else
        {
            playerMovementAnimator.SetBool("Sliding", false);
            Vector2 position = player.position + moveSpeed * Time.fixedDeltaTime * _movement;
            player.MovePosition(position);
        }
    }
}
