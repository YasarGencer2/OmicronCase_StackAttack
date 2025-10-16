using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int health;
    int stack;
    [SerializeField] TargetColor color;
    Color unityColor;

    [Space(10)]
    [SerializeField] TextMeshPro healthText;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector3 stackOffset = new Vector3(0.2f, 0.2f, 0f);
    [SerializeField] Vector3 stackTextOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] TargetColors TargetColors;
    [SerializeField] List<SpriteRenderer> stacks = new();

    [SerializeField] SpriteRenderer topRenderer;
    int requiredHealthForStackChange;
    int lastHealthBeforeStackChange;

    void OnValidate()
    {
        SetStackAmount();
        SetHealth();
        SetHealthText();
        SetColor();
        SetStackVisual();
    }

    void SetColor()
    {
        unityColor = TargetColors.GetColor(color);
        foreach (var item in stacks)
        {
            item.color = unityColor;
        }
    }
    void SetStackAmount()
    {
        stack = health;
        if (stack < 1)
            stack = 1;
        if (health > 10)
            stack /= 2;
        // if (health > 20)
        //     stack /= 2;
    }
    void SetStackVisual()
    {
        var lastR = spriteRenderer;
        for (int i = 0; i < stack; i++)
        {
            var stackIndex = i;
            if (stacks.Count <= stackIndex)
            {
                var clone = Instantiate(lastR, lastR.transform);
                clone.transform.localPosition = stackOffset;
                clone.transform.localScale = Vector3.one;
                stacks.Add(clone);
                lastR = clone;
                continue;
            }
            var stack = stacks[stackIndex];
            stack.transform.localScale = Vector3.one;
            lastR = stack;
        }
        for (int i = stack; i < stacks.Count; i++)
        {
            stacks[i].transform.localScale = Vector3.zero;
        }
        topRenderer = lastR;
    }
    void SetHealth()
    {
        if (health <= 0)
            health = 1;

        requiredHealthForStackChange = health / stack;
        lastHealthBeforeStackChange = health;
    }
    void SetHealthText()
    {
        var localPosition = healthText.transform.localPosition;
        localPosition.y = stackTextOffset.y * stack;
        healthText.transform.localPosition = localPosition;
        UpdateHealthText();
    }
    private void UpdateHealthText()
    {
        if (healthText == null)
            return;
        if (health < 0)
            return;
        healthText.text = health.ToString();

        healthText.DOKill();
        healthText.transform.localScale = Vector3.one;
        healthText.transform.DOScale(Vector3.one * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateTopRenderer();
        SetHealthText();
        PunchAll();
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
        var dieParticle = ParticleSystem.Instance.Play(ParticleType.TargetKilled, transform.position);
        var mainModule = dieParticle.main;
        mainModule.startColor = unityColor;
    }
    void UpdateTopRenderer()
    {
        if (health <= lastHealthBeforeStackChange - requiredHealthForStackChange && stack > 1)
        {
            stack--;
            if (stack < 0)
                stack = 0;
            lastHealthBeforeStackChange = health;
            topRenderer.gameObject.SetActive(false);
            topRenderer = stacks[stack - 1];
        }
        topRenderer.DOKill();
        topRenderer.color = Color.white;
        topRenderer.DOColor(unityColor, 0.1f).SetDelay(0.05f);
    }
    void PunchAll()
    {
        if (transform == null)
            return;
        if (health <= 0)
            return;
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOScaleY(1.1f, 0.1f).OnComplete(() =>
        {
            transform.DOScaleY(1f, 0.1f);
        });
    }
}
