using DG.Tweening;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField] SpriteRenderer red, yellow, white;
    [SerializeField] Collider2D collider2D;

    int damage;
    void OnEnable()
    {
        red.transform.DOScale(1, 0.2f).From(0f).SetEase(Ease.InSine).SetDelay(0f);
        yellow.transform.DOScale(1, 0.2f).From(0f).SetEase(Ease.InSine).SetDelay(0.05f);
        white.transform.DOScale(1, 0.2f).From(0f).SetEase(Ease.InSine).SetDelay(0.1f);

        red.DOFade(0, 0.2f).SetDelay(0.35f);
        yellow.DOFade(0, 0.2f).SetDelay(0.35f);
        white.DOFade(0, 0.2f).SetDelay(0.35f).OnComplete(() => gameObject.SetActive(false));

        collider2D.enabled = true;
    }
    void OnDisable()
    {
        red.transform.localScale = Vector3.zero;
        yellow.transform.localScale = Vector3.zero;
        white.transform.localScale = Vector3.zero;

        red.DOFade(1, 0f);
        yellow.DOFade(1, 0f);
        white.DOFade(1, 0f);

        collider2D.enabled = false;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Target target = collider.gameObject.GetComponentInParent<Target>();
        if (target != null)
        { 
            target.TakeDamage(damage);
        }
    }
}
