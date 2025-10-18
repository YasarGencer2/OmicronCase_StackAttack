using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public UnityAction OnTargetKilled;
    public void Trigger_TargetKilled() => OnTargetKilled?.Invoke();

    public UnityAction<int, int> OnXPChange;
    public UnityAction OnLevelUp;
    public void Trigger_XPChange(int currentXP, int requiredXP) => OnXPChange?.Invoke(currentXP, requiredXP);
    public void Trigger_LevelUp() => OnLevelUp?.Invoke();

    public UnityAction OnHealtLost;
    public void Trigger_HealthLost() => OnHealtLost?.Invoke();

    public UnityAction OnLevelCompleted;
    public UnityAction OnLevelFailed;
    public UnityAction OnLevelLoaded;
    public UnityAction OnLevelLoadStarted;
    public void Trigger_LevelCompleted() => OnLevelCompleted?.Invoke();
    public void Trigger_LevelFailed() => OnLevelFailed?.Invoke();
    public void Trigger_LevelLoaded() => OnLevelLoaded?.Invoke();
    public void Trigger_LevelLoadStarted() => OnLevelLoadStarted?.Invoke();

    public UnityAction OnFirstInput;
    public void Trigger_FirstInput() => OnFirstInput?.Invoke();

    public UnityAction<UpgradeCard> OnCardSelected;
    public void Trigger_OnCardSelected(UpgradeCard upgradeCard) => OnCardSelected?.Invoke(upgradeCard);

    public UnityAction OnRowDied;
    public void Trigger_RowDied() => OnRowDied?.Invoke();

    public UnityAction OnBossSpawned;
    public UnityAction<int> OnBossDamaged;
    public void Trigger_BossSpawned() => OnBossSpawned?.Invoke();
    public void Trigger_BossDamaged(int currentHealth) => OnBossDamaged?.Invoke(currentHealth);
}
