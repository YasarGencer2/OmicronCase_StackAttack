using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public static bool OPEN = false;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] List<UpgradeCard> cards;
    void Awake()
    {
        OPEN = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelUp += Show;
    }

    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelUp -= Show;
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
        OPEN = false;
        canvasGroup.DOFade(0, 0.25f).SetDelay(delay).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }


    void AskForUpgrades()
    {
        var upgrades = WeaponUpgrades.GenerateUpgrades(cards.Count);
        CreateCards(upgrades);
    }
    void CreateCards(List<WeaponUpgrade> upgrades)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetUpgrade(upgrades[i], OnCardSelected);
        }
    }
    void OnCardSelected(UpgradeCard card)
    {
        foreach (var item in cards)
        {
            if (card != item)
                item.Hide();
        }
        Hide(.5f);
    }
}
