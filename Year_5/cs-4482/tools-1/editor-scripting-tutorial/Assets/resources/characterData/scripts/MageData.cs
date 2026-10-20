using Types;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mage Data", menuName = "Character Data/Mage")]
public class MageData : CharacterData
{
    public MageDamageType damageType;
    public MageWeaponType weaponType;
}
