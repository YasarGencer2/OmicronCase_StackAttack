using System;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUpgradeDatas", menuName = "WeaponUpgradeDatas", order = 1)]
public class WeaponUpgradeDatas : ScriptableObject
{
    public WeaponUpgradeData[] Data;

    public WeaponUpgradeWrapper GetRandomUpgrade(Weapon weapon)
    {
        var randomIndex = UnityEngine.Random.Range(0, Data.Length);
        var upgradeData = Data[randomIndex];
        var wrapper = DataToWrapper(upgradeData, weapon);
        return wrapper;
    }
    WeaponUpgradeWrapper DataToWrapper(WeaponUpgradeData data, Weapon weapon)
    {
        var wrapper = new WeaponUpgradeWrapper();
        wrapper.Weapon = weapon;
        wrapper.Type = data.Type;
        wrapper.Rarity = GetRarity();
        wrapper.IsPercent = data.IsPercent;
        switch (wrapper.Rarity)
        {
            case WURarity.Common:
                wrapper.Amount = UnityEngine.Random.Range(data.MinMaxCommon.x, data.MinMaxCommon.y);
                break;
            case WURarity.Rare:
                wrapper.Amount = UnityEngine.Random.Range(data.MinMaxRare.x, data.MinMaxRare.y);
                break;
            case WURarity.Epic:
                wrapper.Amount = UnityEngine.Random.Range(data.MinMaxEpic.x, data.MinMaxEpic.y);
                break;
            case WURarity.Legendary:
                wrapper.Amount = UnityEngine.Random.Range(data.MinMaxLegendary.x, data.MinMaxLegendary.y);
                break;
        }
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