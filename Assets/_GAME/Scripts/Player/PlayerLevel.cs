using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] int currentLevel = 0;
    [SerializeField] int xpPoints = 0;
    [SerializeField] int requiredXP = 5;
    [SerializeField] int xpIncrementPercentage = 20;

    void OnEnable()
    {
        GameEventSystem.Instance.OnTargetKilled += TargetKilled;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnTargetKilled -= TargetKilled;
    }

    private void TargetKilled()
    {
        GainXp();
    }
    private void LevelUp()
    {
        currentLevel++;
        xpPoints = 0;
        requiredXP += Mathf.CeilToInt(requiredXP * (xpIncrementPercentage / 100f));
        GameEventSystem.Instance.Trigger_LevelUp();
    }
    void GainXp()
    {
        xpPoints++;
        if (xpPoints >= requiredXP)
        {
            LevelUp();
        }
        GameEventSystem.Instance.Trigger_XPChange(xpPoints, requiredXP);
    }
}
