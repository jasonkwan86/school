using UnityEngine;

public abstract class Pickup : ScriptableObject
{
    public abstract void ApplyEffect(Player player);
}
