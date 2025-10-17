using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform hand;

    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput += FirstInput;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput -= FirstInput;
    }
    void LevelLoadStarted()
    {
        canvasGroup.DOFade(1, 0.2f);
        HandLoop();
    }
    void FirstInput()
    {
        canvasGroup.DOFade(0, 0.2f);
    }
    void Awake()
    {
        canvasGroup.alpha = 1;
    }
    void HandLoop()
    {
        hand.DOKill();
        hand.localPosition = new Vector3(0, 0, 0);
        hand.DOLocalMoveX(300, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
