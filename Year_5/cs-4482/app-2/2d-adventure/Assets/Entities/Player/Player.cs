using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    
    private void Start()
    {
        hurtbox.OnHurt += HandleHurt;
    }

    private void HandleHurt()
    {
        GameManager.RestartLevel();
    }
}
