using UnityEngine;

[CreateAssetMenu(fileName = "HealthPickup", menuName = "ScriptableObjects/Pickup", order = 1)]
public class HealthPickupEffect : Pickup
{ 
    [SerializeField] private int healthToReplenish = 1;
    
    public override void ApplyEffect(Player player)
    {
        player.TakeHealthPickup(healthToReplenish);
    }
}
