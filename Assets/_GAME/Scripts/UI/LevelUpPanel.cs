using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public static bool OPEN = false;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] List<UpgradeCard> cards;

    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelUp += Show;
        GameEventSystem.Instance.OnLevelLoadStarted += LevelLoadStarted;
        GameEventSystem.Instance.OnCardSelected += OnCardSelected;
    }

    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelUp -= Show;
        GameEventSystem.Instance.OnLevelLoadStarted -= LevelLoadStarted;
        GameEventSystem.Instance.OnCardSelected -= OnCardSelected;
    }
    void LevelLoadStarted()
    {
        OPEN = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Show()
    {
        OPEN = true;
        canvasGroup.DOFade(1, 0.25f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        AskForUpgrades();

    }
    void Hide(float delay)
    {
        canvasGroup.DOFade(0, 0.25f).SetDelay(delay).OnComplete(() =>
        {
            OPEN = false;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }


    void AskForUpgrades()
    {
        var upgrades = WeaponUpgrades.GenerateUpgrades(cards.Count);
        CreateCards(upgrades);
    }
    void CreateCards(List<WeaponUpgradeWrapper> upgrades)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetUpgrade(upgrades[i]);
        }
    }
    void OnCardSelected(UpgradeCard card)
    {
        foreach (var item in cards)
        {
            if (card != item)
                item.Hide();
        }
        Hide(.25f);
    }
}
