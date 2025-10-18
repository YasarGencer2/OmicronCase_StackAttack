using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] int currentLevel = 0;
    [SerializeField] int xpPoints = 0;
    [SerializeField] int requiredXP = 5;
    [SerializeField] int xpIncrementPercentage = 20;
    int currentRequiredXP;

    bool canGainXP = true;

    void OnEnable()
    {
        GameEventSystem.Instance.OnTargetKilled += TargetKilled;
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnBossSpawned += BossSpawned;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnTargetKilled -= TargetKilled;
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnBossSpawned -= BossSpawned;
    }

    private void LevelLoadStarted()
    {
        canGainXP = true;
        currentLevel = 0;
        xpPoints = 0;
        currentRequiredXP = requiredXP;
        GameEventSystem.Instance.Trigger_XPChange(xpPoints, requiredXP);
    }
    private void TargetKilled()
    {
        if (!canGainXP)
            return;
        GainXp();
    }
    void BossSpawned()
    {
        canGainXP = false;
    }
    private void LevelUp()
    {
        currentLevel++;
        xpPoints = 0;
        currentRequiredXP += Mathf.CeilToInt(currentRequiredXP * (xpIncrementPercentage / 100f));
        GameEventSystem.Instance.Trigger_LevelUp();
    }
    void GainXp()
    {
        xpPoints++;
        if (xpPoints >= currentRequiredXP)
        {
            LevelUp();
        }
        GameEventSystem.Instance.Trigger_XPChange(xpPoints, currentRequiredXP);
    }
}
