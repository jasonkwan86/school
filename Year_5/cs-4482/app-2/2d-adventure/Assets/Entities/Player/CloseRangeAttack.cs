using UnityEngine;

public class CloseRangeAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 20;

    public void Execute(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, LayerMask.GetMask("Enemy"));
        
        Debug.DrawRay(transform.position, direction * attackRange, Color.red, 1f);
        if (!hit)
        {
            return;
        }
        Hurtbox hurtbox = hit.collider.GetComponent<Hurtbox>();
        if (hit.collider !=null && hurtbox)
        {
            hurtbox.EvokeOnHurt();
        }
    }
}
