using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Target target;
    Boss boss;

    float timer = 0;

    void OnEnable()
    {
        boss = LevelManager.Instance.Level.Boss;
        timer = 0;
    }

    void Start()
    {
        SetTarget();
    }
    void SetTarget()
    {
        target.IsBoss = true;
        target.SetData(boss.MaxHealth, boss.Color, 5);
        target.Start();
    }

    void Update()
    {
        if (GameHelper.Instance.IsGamePlaying() == false) return;
        if (target == null) return;
        if (target.health <= 0) return;

        TrySendRow();
    }
    void TrySendRow()
    {
        timer += Time.deltaTime;
        if (timer >= 1 / boss.FireRate)
        {
            timer = 0;
            SendRow();
        }
    }
    void SendRow()
    {
        var row = LevelManager.Instance.GetFromPool();

        var rowData = CreateRowData();
        var origin = transform.position + new Vector3(0, 8f, 0);
        row.Set(rowData, origin, false);

        var endPos = GameHelper.Instance.DespawnY * Vector3.up;
        var time = Mathf.Abs(endPos.y - origin.y) / boss.RowMoveSpeed;
        var time1 = time * 0.2f;
        var time2 = time * 0.8f;
        row.transform.DOMoveY(transform.position.y - 2, time1).SetEase(Ease.Linear);
        row.transform.DOMoveY(endPos.y, time2).SetDelay(time1).SetEase(Ease.Linear).OnComplete(() =>
        {
            row.Die?.Invoke(row);
        });
    }

    private Row CreateRowData()
    {
        var row = new Row();

        int amount = boss.RowAmount;
        var randomIndices = BossManager.GetRandomIndexes(amount, 6);
        row.rowHelpers = new RowHelper[6];

        for (int i = 0; i < 6; i++)
        {
            var rowHelper = new RowHelper();
            if (randomIndices.Contains(i))
            {
                rowHelper.color = TargetColors.GetRandomColor();
                rowHelper.stack = boss.RowStack;
            }
            else
            {
                rowHelper.stack = 0;
                rowHelper.color = TargetColor.None;
            }
            row.rowHelpers[i] = rowHelper;
        }

        return row;
    }
}