using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int maxHealth = 5;
    public InputAction MoveAction;
    public float speed = 3.0f;
    public int health { get { return currentHealth; }}
    public int currentHealth;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    Animator movement_animator;
    public Animator wrench_animator;
    public Transform wrench;
    public SpriteRenderer wrenchSprite;
    Vector2 moveDirection = new Vector2(1,0);
    public InputAction talkAction;
    private Vector2 faceDirection;
    
    private void Start()
    {
        wrenchSprite.enabled = false;
        currentHealth = maxHealth;
        MoveAction.Enable();
        talkAction.Enable();
        move = MoveAction.ReadValue<Vector2>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        movement_animator = GetComponent<Animator>();
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
            faceDirection = moveDirection;
            wrench.rotation = Quaternion.LookRotation(Vector3.forward, -faceDirection);
        }
        movement_animator.SetFloat("Look X", moveDirection.x);
        movement_animator.SetFloat("Look Y", moveDirection.y);
        movement_animator.SetFloat("Speed", move.magnitude);
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            MeleeWrench();
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + 3.0f * speed * Time.deltaTime * move;
        rigidbody2d.MovePosition(position);
    }
    
    public void ChangeHealth (int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            movement_animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        movement_animator.SetTrigger("Launch");
    }
    
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position,  faceDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }
        }
    }

    void MeleeWrench()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, faceDirection, 50f, LayerMask.GetMask("Enemy"));
        if (hit.collider != null)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Fix();
            }
        }
        wrenchSprite.enabled = true;
        wrench_animator.Play("WrenchSwing", 0, 0);
    }
}
