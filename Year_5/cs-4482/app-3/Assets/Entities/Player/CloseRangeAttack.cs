using System;
using System.Collections;
using UnityEngine;

public class CloseRangeAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 20;
    [SerializeField] private Transform attackRotationObject;
    [SerializeField] private SpriteRenderer swordSpriteRenderer;
    [SerializeField] private AudioClip swingAudioClip;

    public void Execute(Vector3 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, attackRange, LayerMask.GetMask("Enemy"));

        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        attackRotationObject.rotation = Quaternion.Euler(new Vector3(0, 0, angleDegrees));
        swordSpriteRenderer.enabled = true;
        StartCoroutine(ExecuteAfterDelay(0.5f, () => { swordSpriteRenderer.enabled = false; }));
        // Debug.DrawRay(transform.position, direction * attackRange, Color.red, 1f);
        FindObjectOfType<AudioSource>().PlayOneShot(swingAudioClip);
        if (hits.Length == 0)
        {
            return;
        }
        foreach (RaycastHit2D hit in hits)
        {
            Hurtbox hurtbox = hit.collider.GetComponent<Hurtbox>();
            if (hit.collider !=null&& hurtbox)
            {
                hurtbox.EvokeOnHurt();
            }
        }
    }
    
    IEnumerator ExecuteAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
