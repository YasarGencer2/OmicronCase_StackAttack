using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI title, desc;
    [SerializeField] Button button;
    public void SetUpgrade(WeaponUpgrade weaponUpgrade, Action<UpgradeCard> onCardSelected)
    {
        canvasGroup.alpha = 1;
        icon.sprite = weaponUpgrade.Weapon.Icon;
        transform.localScale = Vector3.one;
        SetTexts(weaponUpgrade);
        SetButton(onCardSelected, weaponUpgrade);
    }
    void SetTexts(WeaponUpgrade weaponUpgrade)
    {
        var name = weaponUpgrade.Weapon.Name;
        switch (weaponUpgrade.Type)
        {
            case WeaponUpgradeTypes.Unlock:
                title.text = $"UNLOCK {name}";
                desc.text = "";
                break;
            case WeaponUpgradeTypes.Damage:
                title.text = $"DAMAGE";
                desc.text = weaponUpgrade.IsPercent ? $"+{weaponUpgrade.Amount}%" : $"+{weaponUpgrade.Amount}";
                break;
            case WeaponUpgradeTypes.Range:
                title.text = $"RANGE";
                desc.text = weaponUpgrade.IsPercent ? $"+{weaponUpgrade.Amount}%" : $"+{weaponUpgrade.Amount}";
                break;
            case WeaponUpgradeTypes.FireRate:
                title.text = $"FIRE RATE";
                desc.text = weaponUpgrade.IsPercent ? $"+{weaponUpgrade.Amount}%" : $"+{weaponUpgrade.Amount}";
                break;
            case WeaponUpgradeTypes.Speed:
                title.text = $"SPEED";
                desc.text = weaponUpgrade.IsPercent ? $"+{weaponUpgrade.Amount}%" : $"+{weaponUpgrade.Amount}";
                break;
            case WeaponUpgradeTypes.ProjectileCount:
                title.text = $"PROJECTILE COUNT";
                desc.text = weaponUpgrade.IsPercent ? $"+{weaponUpgrade.Amount}%" : $"+{weaponUpgrade.Amount}";
                break;
            case WeaponUpgradeTypes.Pierce:
                title.text = $"PIERCE";
                desc.text = weaponUpgrade.IsPercent ? $"+{weaponUpgrade.Amount}%" : $"+{weaponUpgrade.Amount}";
                break;
        }
    }
    void SetButton(Action<UpgradeCard> onCardSelected, WeaponUpgrade upgrade)
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
    public void Hide()
    {
        canvasGroup.DOFade(.2f, 0.2f);
        button.interactable = false;
    }
}
