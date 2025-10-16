using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Projectile prefab;

    [Space(10)]
    [SerializeField] float damage = 1f;
    [SerializeField] float range = 1f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] float projectileCount = 1f;
    [SerializeField] float pierce = 1;

    [Space(10)]
    public float DamageBoost = 1;
    public float RangeBoost = 1;
    public float FireRateBoost = 1;
    public float SpeedBoost = 1;
    public float ProjectileCountBoost = 0;
    public float PierceBoost = 0;

    public float Damage => damage * DamageBoost;
    public float Range => range * RangeBoost;
    public float FireRate => fireRate * FireRateBoost;
    public float Speed => speed * SpeedBoost;
    public float ProjectileCount => projectileCount + ProjectileCountBoost;
    public float Pierce => pierce + PierceBoost;


    float lastFireTime = 0f;

    public void Reset()
    {
        lastFireTime = 0f;
    }
    public bool TryFire(float tickTime)
    {
        if (tickTime - lastFireTime >= 1f / fireRate)
        {
            lastFireTime = tickTime;
            return true;
        }
        return false;
    }

    public void Fire()
    {
        var projectile = Instantiate(prefab);
        projectile.Initialize(this);
    }
}
