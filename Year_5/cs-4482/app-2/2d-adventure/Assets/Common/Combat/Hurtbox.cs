using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hurtbox : MonoBehaviour
{
    public event Action OnHurt;

    public void EvokeOnHurt()
    {
        OnHurt?.Invoke();
    }
}
