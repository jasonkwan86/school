using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ExploderComponent : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float explodeDelay = 1f;
    [SerializeField] private GameObject explodeParticles;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip hissAudioClip;

    private bool _playerIsOverlapping;
    private bool _hasStartedExploding;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Player>())
        {
            return;
        }

        if (_hasStartedExploding) return;
        _hasStartedExploding = true;
        FindAnyObjectByType<AudioSource>().PlayOneShot(hissAudioClip);
        _playerIsOverlapping = true;
        StartCoroutine(ExecuteAfterDelay(explodeDelay, HandleExploded));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<Player>())
        {
            return;
        }
        _playerIsOverlapping = false;
    }

    private void HandleExploded()
    {
        if (_playerIsOverlapping)
        {
            GameManager.RestartLevel();
        }
        Destroy(enemy);
        Instantiate(explodeParticles, enemy.transform.position, Quaternion.identity);
    }
    
    IEnumerator ExecuteAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
