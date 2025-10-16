using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;


public class PlayerWeapons : MonoBehaviour
{
    public static PlayerWeapons Instance { get; private set; }

    [SerializeField] Transform weaponPunch;
    [SerializeField] List<Weapon> weapons;
    public List<Weapon> Weapons => weapons;
    float tickTime;

    void Awake()
    {
        Instance = this;

        weapons = weapons.Where(w => w != null).Select(w => Instantiate(w)).ToList();
    }
    void Start()
    {
        tickTime = 0f;
        foreach (var item in weapons)
        {
            item.Reset();
        }
    }
    void Update()
    {
        if (GameHelper.Instance.IsGamePlaying() == false)
            return;
        tickTime += Time.deltaTime;
        TryFireAll();
    }
    void TryFireAll()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            var weapon = weapons[i];
            if (weapon.TryFire(tickTime))
            {
                weapon.Fire();
                if (i == 0)
                    FireTween();
            }
        }
    }

    public void Upgrade(WeaponUpgrade upgrade)
    {
        var weapon = weapons.Find(w => w.Name == upgrade.Weapon.Name);
        if (weapon == null)
        {
            if (upgrade.Type == WeaponUpgradeTypes.Unlock)
            {
                weapons.Add(Instantiate(upgrade.Weapon));
            }
            return;
        }
        else
        {
            var upgradeAmount = upgrade.IsPercent ? upgrade.Amount / 100f : upgrade.Amount;
            switch (upgrade.Type)
            {
                case WeaponUpgradeTypes.Damage:
                    weapon.DamageBoost += upgradeAmount;
                    break;
                case WeaponUpgradeTypes.Range:
                    weapon.RangeBoost += upgradeAmount;
                    break;
                case WeaponUpgradeTypes.FireRate:
                    weapon.FireRateBoost += upgradeAmount;
                    break;
                case WeaponUpgradeTypes.Speed:
                    weapon.SpeedBoost += upgradeAmount;
                    break;
                case WeaponUpgradeTypes.ProjectileCount:
                    weapon.ProjectileCountBoost += upgradeAmount;
                    break;
                case WeaponUpgradeTypes.Pierce:
                    weapon.PierceBoost += upgradeAmount;
                    break;
            }
        }
    }
    void FireTween()
    {
        if(weaponPunch == null)
            return;
        weaponPunch.DOKill();
        weaponPunch.localScale = Vector3.one;
        weaponPunch.DOPunchScale(Vector3.one * -0.2f, 0.2f, 1, 0);
    }
}



