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
    [SerializeField] Slider bossSlider;
    [SerializeField] TextMeshProUGUI bossIncomingAlert;

    void OnEnable()
    {
        GameEventSystem.Instance.OnXPChange += ChangeXPSlider;
        GameEventSystem.Instance.OnHealtLost += HealtLost;
        GameEventSystem.Instance.OnLevelLoaded += OnLevelLoaded;
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput += FirstInput;
        GameEventSystem.Instance.OnBossSpawned += BossSpawned;
        GameEventSystem.Instance.OnBossDamaged += ChangeBossSlider;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnXPChange -= ChangeXPSlider;
        GameEventSystem.Instance.OnHealtLost -= HealtLost;
        GameEventSystem.Instance.OnLevelLoaded -= OnLevelLoaded;
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnFirstInput -= FirstInput;
        GameEventSystem.Instance.OnBossSpawned -= BossSpawned;
        GameEventSystem.Instance.OnBossDamaged -= ChangeBossSlider;
    }
    void OnLevelLoaded()
    {
        SetSlider();
        SetHearts();
        SetLevelText();
        bossIncomingAlert.DOFade(0, 0);
    }
    void LevelLoadStarted()
    {
        canvasGroup.DOFade(0, 0.2f);
        bossSlider.GetComponent<CanvasGroup>().DOFade(0, 0);
    }
    void FirstInput()
    {
        canvasGroup.DOFade(1, 0.2f);
    }
    void BossSpawned()
    {
        xp.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        bossSlider.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

        bossSlider.maxValue = LevelManager.Instance.Level.Boss.MaxHealth;
        bossSlider.value = bossSlider.maxValue;

        BossIncomingAlert();
    }
    void ChangeBossSlider(int arg0)
    {
        bossSlider.value = arg0;
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
    void BossIncomingAlert()
    {
        bossIncomingAlert.DOKill();
        bossIncomingAlert.alpha = 1;
        bossIncomingAlert.DOFade(.7f, .2f).SetLoops(3, LoopType.Yoyo);
        bossIncomingAlert.DOFade(0, 1f).SetDelay(1f);
    }
}
