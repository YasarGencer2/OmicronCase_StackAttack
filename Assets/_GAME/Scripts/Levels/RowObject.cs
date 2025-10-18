using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RowObject : MonoBehaviour
{
    Row rowData;
    [SerializeField] private List<Target> targets = new();

    Tween moveT;

    public Action<RowObject> Die { get; set; }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Reset();
    }
    public void Set(Row row, Vector3 origin, bool localOrigin = true)
    {
        if (row == null)
            return;
        if (row.rowHelpers == null)
            return;
        if (row.rowHelpers.Length != targets.Count)
            return;
        rowData = row;
        transform.DOKill();
        if (localOrigin)
            transform.localPosition = origin;
        else
            transform.position = origin;
        Move();
        for (int i = 0; i < targets.Count; i++)
        {
            var tg = targets[i];
            var rowHelper = row.rowHelpers[i];
            if (rowHelper.stack < 1)
                continue;
            tg.gameObject.SetActive(true);
            tg.SetData(rowHelper.stack, rowHelper.color);
            tg.Start();
        }
        gameObject.SetActive(true);
    }
    void Update()
    {
        if (transform.position.y < GameHelper.Instance.DespawnY)
        {
            Die?.Invoke(this);
        }
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelUp += Pause;
        GameEventSystem.Instance.OnCardSelected += UnPause;
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelUp -= Pause;
        GameEventSystem.Instance.OnCardSelected -= UnPause;
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
    }
    void LevelLoadStarted()
    {
        Die?.Invoke(this);
    }
    void Pause()
    {
        moveT?.Pause();
    }
    void UnPause(UpgradeCard arg0)
    {
        moveT?.Play();
    }

    void Reset()
    {
        foreach (var target in targets)
        {
            target.gameObject.SetActive(false);
        }
        moveT?.Kill();
    }
    void Move()
    {
        if (rowData.MovingRow == false)
            return;
        var pos1 = (Vector3)rowData.Pos1 + transform.localPosition;
        var pos2 = (Vector3)rowData.Pos2 + transform.localPosition;
        transform.localPosition = pos1;
        moveT = transform.DOLocalMove(pos2, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
