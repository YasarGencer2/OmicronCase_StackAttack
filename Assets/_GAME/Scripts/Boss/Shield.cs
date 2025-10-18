using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] List<Target> shieldTargets = new();

    void Start()
    {
        SetShieldTargets();
        SetSpeed();
    }
    void SetShieldTargets()
    {
        var activeLevel = LevelManager.Instance.Level;
        var boss = activeLevel.Boss;

        int amount = boss.ShieldingAmount;
        bool random = boss.ShiledRandomized;

        List<int> indexes = new List<int>();
        if (random)
            indexes = BossManager.GetRandomIndexes(amount, shieldTargets.Count);
        else
            for (int i = 0; i < amount; i++)
                indexes.Add(i);

        for (int i = 0; i < shieldTargets.Count; i++)
        {
            var tg = shieldTargets[i];
            if (indexes.Contains(i))
            {
                tg.gameObject.SetActive(true);
                tg.SetData(boss.ShieldingStack, boss.ShieldColor, 10);
                tg.Start();
            }
            else
            {
                tg.gameObject.SetActive(false);
            }
        }
    }
    void SetSpeed()
    {
        var speed = LevelManager.Instance.Level.Boss.ShieldSpeed;
        if(TryGetComponent<TargetsRotater>(out var rotator))
        {
            rotator.Speed = speed;
        }
    }
}