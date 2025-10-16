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
}
