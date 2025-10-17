using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] TrailRenderer trail;
    public Weapon Weapon { get; private set; }
    public Action<Projectile> Die;

    protected int pierce = 0;
    protected float totalAliveTime = 0f;
    protected bool autoKill = true;

    public virtual void Initialize(Weapon weapon, Vector3 offset)
    {
        this.Weapon = weapon;
        pierce = Mathf.FloorToInt(weapon.Pierce);
        trail.emitting = false;
        transform.position = GameHelper.Instance.PTransform.position + offset;
        trail.Clear();
        trail.emitting = true;
        totalAliveTime = totalAliveTime == 0 ? weapon.Range / weapon.Speed : totalAliveTime; 
        DestroyOnComlpete();
    }
    void DestroyOnComlpete()
    {
        if (autoKill)
            AutoDie(totalAliveTime);
    }
    protected void AutoDie(float time)
    {
        GameHelper.Instance.StartCoroutine(LocalDelay());
        IEnumerator LocalDelay()
        {
            yield return new WaitForSeconds(time);
            if (gameObject.activeSelf)
                Die(this);
        }
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted += OnLevelLoadStarted;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted -= OnLevelLoadStarted;
    }
    private void OnLevelLoadStarted()
    {
        Die(this);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Target target = collider.gameObject.GetComponentInParent<Target>();
        if (target != null)
        {
            InflictDamage(target);
        }
    }
    void Update()
    {
        if (Weapon == null)
            return;
        Move();
    }
    protected virtual void Move()
    {
        transform.Translate(Vector2.up * Weapon.Speed * Time.deltaTime);
    }
    protected virtual void InflictDamage(Target target)
    {
        if (target == null)
            return;
        target.TakeDamage(Weapon.Damage);
        pierce--;
        if (pierce < 0)
            Die(this);
    }

}