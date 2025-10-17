using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startHealth = 3;
    [SerializeField] int invincibilityDuration = 4;
    [SerializeField] Transform invincibilityVisual;
    int currentHealth;
    float invincibilityTimer;

    private void OnLevelLoaded()
    {
        currentHealth = startHealth;
        invincibilityTimer = 0;
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoaded += OnLevelLoaded;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoaded -= OnLevelLoaded;
    }
    void Update()
    {
        if (GameHelper.Instance.IsGamePlaying() == false)
            return;

        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            invincibilityVisual.gameObject.SetActive(true);
        }
        else
        {
            invincibilityVisual.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInvincible())
            return;
        var target = collision.GetComponentInParent<Target>();
        if (target != null)
        {
            target.TakeDamage(1000);
            TakeDamage();
        }
    }
    void TakeDamage()
    {
        currentHealth--;
        GameEventSystem.Instance.Trigger_HealthLost();
        if (TryDie())
            return;
        GoInvincible();
    }
    void GoInvincible()
    {
        invincibilityTimer = invincibilityDuration;
    }
    bool IsInvincible() => invincibilityTimer > 0;
    bool TryDie()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return true;
        }
        return false;
    }

    private void Die()
    {
        GameEventSystem.Instance.Trigger_LevelFailed();
    }
}
