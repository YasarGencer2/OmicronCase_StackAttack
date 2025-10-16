using UnityEngine;

public class GameHelper : MonoBehaviour
{
    public static GameHelper Instance { get; private set; }

    public Transform PTransform;
    public PlayerWeapons PWeapons;
    public WeaponsList AllWeapons;
    
    void Awake()
    {
        Instance = this;
    }

    public bool IsGamePlaying()
    {
        if (LevelUpPanel.OPEN)
            return false;
        return true;
    }
}
