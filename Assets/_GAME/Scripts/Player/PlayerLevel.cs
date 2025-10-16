using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] int currentLevel = 0;
    [SerializeField] int xpPoints = 0;
    [SerializeField] int requiredXP = 5;
    [SerializeField] int xpIncrementPercentage = 20;
    int currentRequiredXP;

    void OnEnable()
    {
        GameEventSystem.Instance.OnTargetKilled += TargetKilled;
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnTargetKilled -= TargetKilled;
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
    }

    private void LevelLoadStarted()
    {
        currentLevel = 0;
        xpPoints = 0;
        currentRequiredXP = requiredXP;
        GameEventSystem.Instance.Trigger_XPChange(xpPoints, requiredXP);
    }

    private void TargetKilled()
    {
        GainXp();
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
