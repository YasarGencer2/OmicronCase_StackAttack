using System;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUpgradeDatas", menuName = "WeaponUpgradeDatas", order = 1)]
public class WeaponUpgradeDatas : ScriptableObject
{
    public WeaponUpgradeData[] Data;
    public AvailableUpgradesForWeapon[] AvailableUpgrades;

    public WeaponUpgradeWrapper GetRandomUpgrade(Weapon weapon)
    {
        var type = GetType(weapon);
        var data = Array.Find(Data, d => d.Type == type);
        var wrapper = DataToWrapper(data, weapon);
        return wrapper;
    }
    WeaponUpgradeWrapper DataToWrapper(WeaponUpgradeData data, Weapon weapon)
    {
        var wrapper = new WeaponUpgradeWrapper();
        wrapper.Weapon = weapon;
        wrapper.Type = data.Type;
        wrapper.Rarity = GetRarity();
        wrapper.IsPercent = data.IsPercent;
        wrapper.Amount = GetAmount(data, wrapper.Rarity);
        return wrapper;
    }
    WURarity GetRarity()
    {
        var roll = UnityEngine.Random.Range(0, 100);
        if (roll < 50)
            return WURarity.Common;
        else if (roll < 80)
            return WURarity.Rare;
        else if (roll < 95)
            return WURarity.Epic;
        else
            return WURarity.Legendary;
    }
    WUType GetType(Weapon weapon)
    {
        foreach (var available in AvailableUpgrades)
        {
            if (available.Weapon.Name == weapon.Name)
            {
                var randomIndex = UnityEngine.Random.Range(0, available.Upgrades.Length);
                return available.Upgrades[randomIndex];
            }
        }
        return WUType.Damage;
    }
    float GetAmount(WeaponUpgradeData data, WURarity rarity)
    {
        switch (rarity)
        {
            case WURarity.Common:
                return UnityEngine.Random.Range(data.MinMaxCommon.x, data.MinMaxCommon.y);
            case WURarity.Rare:
                return UnityEngine.Random.Range(data.MinMaxRare.x, data.MinMaxRare.y);
            case WURarity.Epic:
                return UnityEngine.Random.Range(data.MinMaxEpic.x, data.MinMaxEpic.y);
            case WURarity.Legendary:
                return UnityEngine.Random.Range(data.MinMaxLegendary.x, data.MinMaxLegendary.y);
        }
        return 0f;
    }
}

[System.Serializable]
public class WeaponUpgradeData
{
    public WUType Type;
    public int2 MinMaxCommon, MinMaxRare, MinMaxEpic, MinMaxLegendary;
    public bool IsPercent;
}
[System.Serializable]
public class WeaponUpgradeWrapper
{
    public Weapon Weapon;
    public WUType Type;
    public WURarity Rarity;
    public float Amount;
    public bool IsPercent;
}
[System.Serializable]
public class AvailableUpgradesForWeapon
{
    public Weapon Weapon;
    public WUType[] Upgrades;
}
public enum WUType
{
    Unlock = 0,
    Damage = 1,
    Range = 2,
    FireRate = 3,
    Speed = 4,
    ProjectileCount = 5,
    Pierce = 6,
}
public enum WURarity
{
    Common,
    Rare,
    Epic,
    Legendary
}