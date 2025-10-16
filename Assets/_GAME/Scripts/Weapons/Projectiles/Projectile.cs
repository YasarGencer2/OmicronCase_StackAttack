using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    public Weapon Weapon { get; private set; }
    int pierce = 0;

    public void Initialize(Weapon weapon)
    {
        this.Weapon = weapon;
        pierce = Mathf.FloorToInt(weapon.Pierce);
        transform.position = GameHelper.Instance.PTransform.position;
        // Destroy(gameObject, weapon.range / weapon.speed);
    }
    void OnTriggerEnter2D(Collider2D collider)
    { 
        Target target = collider.gameObject.GetComponentInParent<Target>();
        if (target != null)
        {
            InflictDamage(target);
        }
    }
    void OnDestroy()
    { 
    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.Translate(Vector2.up * Weapon.Speed * Time.deltaTime);
    }
    protected virtual void InflictDamage(Target target)
    {
        var dmg = Mathf.FloorToInt(Weapon.Damage);
        target.TakeDamage(dmg);
        pierce--;
        if (pierce < 0)
            Destroy(gameObject);
    }

}