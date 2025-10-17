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
    [SerializeField] Transform levelParent;
    [SerializeField] RowObject rowPrefab;
    [SerializeField] float rowSpacing;

    [SerializeField] int poolSize = 20;
    private Queue<RowObject> pool = new Queue<RowObject>();
    int lastCreatedIndex = 0;

    Level activeLevel;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        LoadCurrentLevel();
    }
    void InitializePool()
    {
        for (int i = pool.Count; i < poolSize; i++)
        {
            var obj = GetFromPool();
            pool.Enqueue(obj);
        }
    }
    public void LoadCurrentLevel() => LoadLevel(CurrentLevel);
    public void LoadLevel(int index)
    {
        GameEventSystem.Instance.Trigger_LevelLoadStarted();
        Load(index);
        GameEventSystem.Instance.Trigger_LevelLoaded();
    }
    void Load(int index)
    {
        if (index < nonLoopingLevels.Levels.Count)
            activeLevel = nonLoopingLevels.Levels[index];
        else
        {
            int loopedIndex = (index - nonLoopingLevels.Levels.Count) % loopingLevels.Levels.Count;
            activeLevel = loopingLevels.Levels[loopedIndex];
        }
        CreateRows();
    }
    void CreateRows()
    {
        for (int i = 0; i < poolSize; i++)
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
