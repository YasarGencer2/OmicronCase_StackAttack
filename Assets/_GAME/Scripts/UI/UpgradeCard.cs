using System;
using DG.Tweening;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI title, desc, rarity;
    [SerializeField] Button button;
    [SerializeField] Image bg;
    [SerializeField] RarityColors[] rarityColors;
    public void SetUpgrade(WeaponUpgradeWrapper weaponUpgrade, Action<UpgradeCard> onCardSelected)
    {
        SetGeneral(weaponUpgrade);
        SetTexts(weaponUpgrade);
        SetButton(onCardSelected, weaponUpgrade);
        SetColor(weaponUpgrade);
    }
    void SetGeneral(WeaponUpgradeWrapper upgrade)
    {
        canvasGroup.alpha = 1;
        icon.sprite = upgrade.Weapon.Icon;
        transform.localScale = Vector3.one;
        rarity.text = upgrade.Rarity.ToString().ToUpper();
    }
    void SetTexts(WeaponUpgradeWrapper upgrade)
    {
        var name = upgrade.Weapon.Name;
        switch (upgrade.Type)
        {
            case WUType.Unlock:
                title.text = $"UNLOCK";
                desc.text = $"{name}";
                break;
            case WUType.Damage:
                title.text = $"DAMAGE";
                desc.text = upgrade.IsPercent ? $"+{upgrade.Amount}%" : $"+{upgrade.Amount}";
                break;
            case WUType.Range:
                title.text = $"RANGE";
                desc.text = upgrade.IsPercent ? $"+{upgrade.Amount}%" : $"+{upgrade.Amount}";
                break;
            case WUType.FireRate:
                title.text = $"FIRE RATE";
                desc.text = upgrade.IsPercent ? $"+{upgrade.Amount}%" : $"+{upgrade.Amount}";
                break;
            case WUType.Speed:
                title.text = $"SPEED";
                desc.text = upgrade.IsPercent ? $"+{upgrade.Amount}%" : $"+{upgrade.Amount}";
                break;
            case WUType.ProjectileCount:
                title.text = $"PROJECTILE COUNT";
                desc.text = upgrade.IsPercent ? $"+{upgrade.Amount}%" : $"+{upgrade.Amount}";
                break;
            case WUType.Pierce:
                title.text = $"PIERCE";
                desc.text = upgrade.IsPercent ? $"+{upgrade.Amount}%" : $"+{upgrade.Amount}";
                break;
        }
    }
    void SetButton(Action<UpgradeCard> onCardSelected, WeaponUpgradeWrapper upgrade)
    {
        button.onClick.RemoveAllListeners();
        button.interactable = true;
        button.onClick.AddListener(() =>
        {
            onCardSelected?.Invoke(this);
            GameHelper.Instance.PWeapons.Upgrade(upgrade);
            button.interactable = false;
            transform.DOScale(1.3f, 0.2f);
        });
    }
    void SetColor(WeaponUpgradeWrapper upgrade)
    {
        foreach (var rc in rarityColors)
        {
            if (rc.Rarity == upgrade.Rarity)
            {
                bg.color = rc.Color;
                return;
            }
        }
    }
    public void Hide()
    {
        canvasGroup.DOFade(.2f, 0.2f);
        button.interactable = false;
    }
}
[System.Serializable]
public struct RarityColors
{
    public WURarity Rarity;
    public Color Color;
}