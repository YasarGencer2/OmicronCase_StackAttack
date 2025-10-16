using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponUpgrades : MonoBehaviour
{
    public static List<WeaponUpgrade> GenerateUpgrades(int count)
    {
        List<WeaponUpgrade> upgrades = new List<WeaponUpgrade>();
        var doUnlock = UnlockWeapon();
        if (doUnlock != null)
        {
            upgrades.Add(doUnlock.Value);
            count--;
        }
        for (int i = 0; i < count; i++)
        {
            var upgrade = GenerateRandomUpgrade(upgrades);
            upgrades.Add(upgrade);
        }
        upgrades = upgrades.OrderBy(x => UnityEngine.Random.value).ToList();
        return upgrades;
    }

    private static WeaponUpgrade GenerateRandomUpgrade(List<WeaponUpgrade> upgrades)
    {
        var weapon = GetUpgradableWeapon();
        WeaponUpgrade upgrade = WeaponToUpgrade(weapon);

        WeaponUpgradeTypes type;
        do
            type = (WeaponUpgradeTypes)UnityEngine.Random.Range(1, Enum.GetNames(typeof(WeaponUpgradeTypes)).Length);
        while (upgrades.Exists(x => x.Type == type)); 
        upgrade.Type = type;

        switch (type)
        {
            case WeaponUpgradeTypes.Damage:
                upgrade.Amount = UnityEngine.Random.Range(5, 15);
                upgrade.IsPercent = true;
                break;
            case WeaponUpgradeTypes.Range:
                upgrade.Amount = UnityEngine.Random.Range(5, 15);
                upgrade.IsPercent = true;
                break;
            case WeaponUpgradeTypes.FireRate:
                upgrade.Amount = UnityEngine.Random.Range(5, 15);
                upgrade.IsPercent = true;
                break;
            case WeaponUpgradeTypes.Speed:
                upgrade.Amount = UnityEngine.Random.Range(5, 15);
                upgrade.IsPercent = true;
                break;
            case WeaponUpgradeTypes.ProjectileCount:
                upgrade.Amount = 1;
                upgrade.IsPercent = false;
                break;
            case WeaponUpgradeTypes.Pierce:
                upgrade.Amount = 1;
                upgrade.IsPercent = false;
                break;
        }
        return upgrade;
    }
    static WeaponUpgrade? UnlockWeapon()
    {
        if (UnityEngine.Random.Range(0, 100) > 70)
            return null;
        var weapon = GetUnlockableWeapon();
        if (weapon == null)
            return null;
        return WeaponToUpgrade(weapon);
    }
    static WeaponUpgrade WeaponToUpgrade(Weapon weapon)
    {
        WeaponUpgrade upgrade = new WeaponUpgrade();
        upgrade.Weapon = weapon;
        upgrade.Type = WeaponUpgradeTypes.Unlock;
        upgrade.Amount = 0;
        upgrade.IsPercent = false;
        return upgrade;
    }
    static Weapon GetUpgradableWeapon()
    {
        var playerWeapons = GameHelper.Instance.PWeapons.Weapons;
        if (playerWeapons.Count == 0)
            return null;
        var random = UnityEngine.Random.Range(0, playerWeapons.Count);
        return playerWeapons[random];
    }
    static Weapon GetUnlockableWeapon()
    {
        var allWeapons = GameHelper.Instance.AllWeapons;
        var playerWeapons = GameHelper.Instance.PWeapons.Weapons;

        var unlockableWeapons = allWeapons.Weapons.Where(w => !playerWeapons.Any(pw => pw.Name == w.Name)).ToList();
        if (unlockableWeapons.Count == 0)
            return null;
        var random = UnityEngine.Random.Range(0, unlockableWeapons.Count);
        return unlockableWeapons[random];
    }
}

public struct WeaponUpgrade
{
    public Weapon Weapon;
    public WeaponUpgradeTypes Type;
    public float Amount;
    public bool IsPercent;
}
public enum WeaponUpgradeTypes
{
    Unlock = 0,
    Damage = 1,
    Range = 2,
    FireRate = 3,
    Speed = 4,
    ProjectileCount = 5,
    Pierce = 6,
}