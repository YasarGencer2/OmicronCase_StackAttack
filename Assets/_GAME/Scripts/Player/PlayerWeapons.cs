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
    [SerializeField] List<Weapon> starterWeapons;
    [SerializeField] List<Weapon> weapons;
    public List<Weapon> Weapons => weapons;
    float tickTime;

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnBossSpawned += BossSpawned;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnBossSpawned -= BossSpawned;
    }
    void LevelLoadStarted()
    {
        weapons = starterWeapons.Where(w => w != null).Select(w => Instantiate(w)).ToList();
        tickTime = 0f;
        foreach (var item in weapons)
        {
            item.Reset();
            item.InitializePool();
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

    public void Upgrade(WeaponUpgradeWrapper upgrade)
    {
        var weapon = weapons.Find(w => w.Name == upgrade.Weapon.Name);
        if (weapon == null)
        {
            if (upgrade.Type == WUType.Unlock)
            {
                var wp = Instantiate(upgrade.Weapon);
                wp.Reset();
                wp.InitializePool();
                weapons.Add(wp);
            }
            return;
        }
        else
        {
            var upgradeAmount = upgrade.IsPercent ? upgrade.Amount / 100f : upgrade.Amount;
            switch (upgrade.Type)
            {
                case WUType.Damage:
                    weapon.DamageBoost += upgradeAmount;
                    break;
                case WUType.Range:
                    weapon.RangeBoost += upgradeAmount;
                    break;
                case WUType.FireRate:
                    weapon.FireRateBoost += upgradeAmount;
                    break;
                case WUType.Speed:
                    weapon.SpeedBoost += upgradeAmount;
                    break;
                case WUType.ProjectileCount:
                    weapon.ProjectileCountBoost += upgradeAmount;
                    break;
                case WUType.Pierce:
                    weapon.PierceBoost += upgradeAmount;
                    break;
            }
        }
    }
    void FireTween()
    {
        if (weaponPunch == null)
            return;
        weaponPunch.DOKill();
        weaponPunch.localScale = Vector3.one;
        weaponPunch.DOPunchScale(Vector3.one * -0.2f, 0.2f, 1, 0);
    }
    void BossSpawned()
    {
        tickTime -= 2f;
    }
}



