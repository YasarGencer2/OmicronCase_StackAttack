using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsList", menuName = "WeaponsList")]
public class WeaponsList : ScriptableObject
{
    public List<Weapon> Weapons;
}
