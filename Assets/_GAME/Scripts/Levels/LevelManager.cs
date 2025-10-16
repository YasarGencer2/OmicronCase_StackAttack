using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public static int VisualLevel => CurrentLevel + 1;
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt("CurrentLevel", 0);
        set => PlayerPrefs.SetInt("CurrentLevel", value);
    }
    [SerializeField] LevelList nonLoopingLevels;
    [SerializeField] LevelList loopingLevels;
    [SerializeField] Transform levelParent;
    [SerializeField] Level activeLevel; 

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        LoadCurrentLevel();
    }
    public void LoadCurrentLevel() => LoadLevel(CurrentLevel);
    void DostroyActiveLevel()
    {
        if (activeLevel != null)
            Destroy(activeLevel.gameObject);
    }
    public void LoadLevel(int index)
    {
        GameEventSystem.Instance.Trigger_LevelLoadStarted();
        DostroyActiveLevel();
        activeLevel = GetLevel(index);
        GameEventSystem.Instance.Trigger_LevelLoaded(activeLevel);
    }
    Level GetLevel(int index)
    {
        Level level;
        if (index < nonLoopingLevels.Levels.Count)
            level = Instantiate(nonLoopingLevels.Levels[index], levelParent);
        else
        {
            int loopedIndex = (index - nonLoopingLevels.Levels.Count) % loopingLevels.Levels.Count;
            level = Instantiate(loopingLevels.Levels[loopedIndex], levelParent);
        }
        activeLevel = level;
        return level;
    }
}
