using System.Collections.Generic;
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

    [Space(5)]
    [SerializeField] Transform levelParent;
    [SerializeField] RowObject rowPrefab;
    [SerializeField] float rowSpacing;

    [Space(5)]
    [SerializeField] int poolSize = 20;
    private Queue<RowObject> pool = new Queue<RowObject>();
    int lastCreatedIndex = 0;

    Level activeLevel;
    public Level Level => activeLevel;
    GameObject activeCustom;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        LoadCurrentLevel();
    }
    public void LoadCurrentLevel() => LoadLevel(CurrentLevel);
    public void LoadLevel(int index)
    {
        if (activeCustom != null)
            Destroy(activeCustom);
        Select(index);
        GameEventSystem.Instance.Trigger_LevelLoadStarted();
        Load();
        GameEventSystem.Instance.Trigger_LevelLoaded();
    }
    void Select(int index)
    {
        if (index < nonLoopingLevels.Levels.Count)
            activeLevel = nonLoopingLevels.Levels[index];
        else
        {
            int loopedIndex = (index - nonLoopingLevels.Levels.Count) % loopingLevels.Levels.Count;
            activeLevel = loopingLevels.Levels[loopedIndex];
        }
    }
    void Load()
    {
        CreateRows();
        if (activeLevel.Custom != null)
            activeCustom = Instantiate(activeLevel.Custom, levelParent);
    }
    void CreateRows()
    {
        for (int i = 0; i < Mathf.Min(poolSize, activeLevel.Rows.Length); i++)
        {
            var row = activeLevel.Rows[i];
            CreateRow(row, i);
            lastCreatedIndex = i;
        }
    }
    void CreateRow(Row row, int index)
    {
        RowObject rowObject = GetFromPool();
        var origin = new Vector3(0f, index * rowSpacing, 0f);
        rowObject.Set(row, origin);
        rowObject.Die = ReturnRow;
    }

    void ReturnRow(RowObject rowObject)
    {
        rowObject.Deactivate();
        pool.Enqueue(rowObject);
        GameEventSystem.Instance.Trigger_RowDied();
        TryCreateRow();
    }
    public RowObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        var obj = Instantiate(rowPrefab);
        obj.Deactivate();
        obj.gameObject.transform.SetParent(levelParent, false);
        return obj;
    }
    void TryCreateRow()
    {
        var level = activeLevel;
        int nextIndex = lastCreatedIndex + 1;
        if (nextIndex >= level.Rows.Length)
            return;
        var row = level.Rows[nextIndex];
        CreateRow(row, nextIndex);
        lastCreatedIndex = nextIndex;
    }
}
