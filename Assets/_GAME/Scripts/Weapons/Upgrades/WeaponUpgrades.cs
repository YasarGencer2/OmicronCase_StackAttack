using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponUpgrades : MonoBehaviour
{
    public static List<WeaponUpgradeWrapper> GenerateUpgrades(int count)
    {
        List<WeaponUpgradeWrapper> upgrades = new List<WeaponUpgradeWrapper>();
        var doUnlock = TryUnlockWeapon();
        if (doUnlock != null)
        {
            upgrades.Add(doUnlock);
            count--;
        }
        for (int i = 0; i < count; i++)
        {
            var upgrade = GenerateRandomUpgrade();
            upgrades.Add(upgrade);
        }
        upgrades = upgrades.OrderBy(x => UnityEngine.Random.value).ToList();
        return upgrades;
    }

    private static WeaponUpgradeWrapper GenerateRandomUpgrade()
    {
        var weapon = GetUpgradableWeapon();
        var upgrade = SetRandomUpgrade(weapon); 
        return upgrade;
    }

    static WeaponUpgradeWrapper TryUnlockWeapon()
    {
        if (UnityEngine.Random.Range(0, 100) > 70)
            return null;
        var weapon = GetUnlockableWeapon();
        if (weapon == null)
            return null;
        return WeaponToUpgrade(weapon);
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
    static WeaponUpgradeWrapper WeaponToUpgrade(Weapon weapon)
    {
        WeaponUpgradeWrapper upgrade = new WeaponUpgradeWrapper();
        upgrade.Weapon = weapon;
        upgrade.Type = WUType.Unlock;
        upgrade.Rarity = WURarity.Legendary;
        upgrade.Amount = 0;
        upgrade.IsPercent = false;
        return upgrade;
    }
    static WeaponUpgradeWrapper SetRandomUpgrade(Weapon weapon)
    {
        var upgrade = WeaponToUpgrade(weapon);
        var upgradeDatas = GameHelper.Instance.AllUpgrades;
        upgrade = upgradeDatas.GetRandomUpgrade(weapon);
        return upgrade;
    }
}
