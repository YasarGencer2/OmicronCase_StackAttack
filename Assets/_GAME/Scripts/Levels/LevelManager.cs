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
    [SerializeField] GameObject activeLevel;
    [SerializeField] RowObject rowPrefab;
    [SerializeField] float rowSpacing;

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
            Destroy(activeLevel);
    }
    public void LoadLevel(int index)
    {
        GameEventSystem.Instance.Trigger_LevelLoadStarted();
        DostroyActiveLevel();
        activeLevel = GetLevel(index);
        GameEventSystem.Instance.Trigger_LevelLoaded();
    }
    GameObject GetLevel(int index)
    {
        GameObject level;
        if (index < nonLoopingLevels.Levels.Count)
            level = CreateLevel(nonLoopingLevels.Levels[index]);
        else
        {
            int loopedIndex = (index - nonLoopingLevels.Levels.Count) % loopingLevels.Levels.Count;
            level = CreateLevel(loopingLevels.Levels[loopedIndex]);
        }
        return level;
    }
    GameObject CreateLevel(Level level)
    {
        GameObject levelGO = new GameObject($"Level_{VisualLevel}");
        levelGO.transform.SetParent(levelParent, false);
        levelGO.transform.localPosition = Vector3.zero;
        activeLevel = levelGO;
        CreateRows(level);
        return levelGO;
    }
    void CreateRows(Level level)
    {
        int index = 0;
        foreach (var row in level.Rows)
        {
            CreateRow(row, index);
            index++;
        }
    }
    void CreateRow(Row row, int index)
    {
        var rowOBject = Instantiate(rowPrefab);
        rowOBject.gameObject.transform.SetParent(activeLevel.transform, false);
        var origin = new Vector3(0f, index * rowSpacing, 0f);
        rowOBject.Set(row, origin);
    }
}
