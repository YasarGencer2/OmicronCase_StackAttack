using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Slider xp;
    [SerializeField] List<GameObject> hearts;
    [SerializeField] TextMeshProUGUI levelText;

    void OnEnable()
    {
        GameEventSystem.Instance.OnXPChange += ChangeXPSlider;
        GameEventSystem.Instance.OnHealtLost += HealtLost;
        GameEventSystem.Instance.OnLevelLoaded += OnLevelLoaded;
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput += FirstInput;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnXPChange -= ChangeXPSlider;
        GameEventSystem.Instance.OnHealtLost -= HealtLost;
        GameEventSystem.Instance.OnLevelLoaded -= OnLevelLoaded;
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput -= FirstInput;
    }
    void OnLevelLoaded()
    {
        SetSlider();
        SetHearts();
        SetLevelText();
    }
    void LevelLoadStarted()
    {
        canvasGroup.DOFade(0, 0.2f);
    }
    void FirstInput()
    {
        canvasGroup.DOFade(1, 0.2f);
    }
    private void ChangeXPSlider(int arg0, int arg1)
    {
        xp.maxValue = arg1;
        xp.value = arg0;
    }
    void SetSlider()
    {
        xp.maxValue = 100;
        xp.value = 0;
    }
    void SetHearts()
    {

        foreach (var heart in hearts)
        {
            heart.SetActive(true);
        }
    }
    void HealtLost()
    {
        for (int i = hearts.Count - 1; i >= 0; i--)
        {
            if (hearts[i].activeSelf)
            {
                hearts[i].SetActive(false);
                break;
            }
        }
    }
    void SetLevelText()
    {
        levelText.text = $"LEVEL {LevelManager.VisualLevel}";
    }
}
