using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] Transform bossParnet;
    [SerializeField] List<ShiledWrapper> shiledWrappers = new();
    GameObject bossInstance;
    GameObject shieldInstance;
    int rowsToKillBeforeBoss = 1000;
    int rowsKilled;

    bool createdBoss = false;

    Level activeLevel;
    Boss boss;
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnRowDied += RowDied;
        GameEventSystem.Instance.OnLevelCompleted += LevelCompleted;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnRowDied -= RowDied;
        GameEventSystem.Instance.OnLevelCompleted -= LevelCompleted;
    }
    void LevelLoadStarted()
    {
        Kill();

        activeLevel = LevelManager.Instance.Level;
        boss = activeLevel.Boss;

        createdBoss = false;
        rowsKilled = 0;
        rowsToKillBeforeBoss = activeLevel.Rows.Length - 3;
    }
    void LevelCompleted()
    {
        Kill();
    }
    void RowDied()
    {
        rowsKilled++; 
        if (rowsKilled >= rowsToKillBeforeBoss)
        {
            SpawnBoss();
        }
    }
    void SpawnBoss()
    {
        if (createdBoss) return;
        createdBoss = true;

        bossInstance = Instantiate(boss.Prefab, bossParnet);
        bossInstance.transform.position = new Vector3(0, GameHelper.Instance.PTransform.position.y + 13f, 0);

        SpawnShield();

        GameEventSystem.Instance.Trigger_BossSpawned();
    }
    void SpawnShield()
    {
        var shieldWrapper = shiledWrappers.Find(x => x.Type == boss.ShieldingType);
        if (shieldWrapper == null) return;
        shieldInstance = Instantiate(shieldWrapper.Prefab, bossParnet);
        shieldInstance.transform.position = bossInstance.transform.position;

    }
    void Kill()
    {
        if (bossInstance != null) Destroy(bossInstance);
        if (shieldInstance != null) Destroy(shieldInstance);
    }

    public static List<int> GetRandomIndexes(int amount, int count)
    {
        List<int> numbers = Enumerable.Range(0, count).ToList();
        System.Random rnd = new System.Random();
        return numbers.OrderBy(x => rnd.Next()).Take(amount).ToList();
    }
}

[System.Serializable]
public class ShiledWrapper
{
    public ShieldingType Type;
    public GameObject Prefab;
}