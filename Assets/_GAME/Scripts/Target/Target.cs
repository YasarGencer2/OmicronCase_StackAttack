using System;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] TextMeshPro healthText;

    void Awake()
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthText();
        TryDie();
    }
    void TryDie()
    {
        if (gameObject.activeSelf == false)
            return;
        if (health > 0)
            return;
        Die();
    }
    void Die()
    {
        gameObject.SetActive(false);
        GameEventSystem.Instance.Trigger_TargetKilled();
    }
}
