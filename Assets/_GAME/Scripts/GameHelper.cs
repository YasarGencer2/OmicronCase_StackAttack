using System;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    public static GameHelper Instance { get; private set; }

    public Transform PTransform;
    public PlayerWeapons PWeapons;
    public WeaponsList AllWeapons;
    public WeaponUpgradeDatas AllUpgrades;
    public TargetColors TargetColors;

    bool didFirstInput = false;

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted += OnLevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput += OnFirstInput;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted -= OnLevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput -= OnFirstInput;
    }
 
    private void OnLevelLoadStarted()
    {
        didFirstInput = false;
    }

    private void OnFirstInput()
    {
        didFirstInput = true;
    }
    public bool IsGamePlaying()
    {
        if (LevelUpPanel.OPEN)
            return false;
        if (LevelEndPanel.OPEN)
            return false;
        if (didFirstInput == false)
            return false;
        return true;
    }
}
