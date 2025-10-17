using System.Collections.Generic;
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

    public int Damage
    {
        get
        {
            var floor = Mathf.FloorToInt(damage * DamageBoost);
            var ceil = Mathf.CeilToInt(damage * DamageBoost);
            var selectRandom = Random.value < .5f ? ceil : floor;
            return selectRandom;
        }
    }
    public float Range => range * RangeBoost;
    public float FireRate => fireRate * FireRateBoost;
    public float Speed => speed * SpeedBoost;
    public int ProjectileCount => Mathf.CeilToInt(projectileCount + ProjectileCountBoost);
    public int Pierce => Mathf.CeilToInt(pierce + PierceBoost);


    float lastFireTime = 0f;

    Queue<Projectile> pool = new Queue<Projectile>();
    int poolSize = 7;

    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var proj = Instantiate(prefab);
            proj.gameObject.SetActive(false);
            pool.Enqueue(proj);
        }
    }
    Projectile GetFromPool()
    {
        if (pool.Count > 0)
        {
            var proj = pool.Dequeue();
            proj.gameObject.SetActive(true);
            return proj;
        }
        var newProj = Instantiate(prefab);
        return newProj;
    }

    void ReturnToPool(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        pool.Enqueue(proj);
    }

    public void Reset()
    {
        lastFireTime = 0f;
    }
    public bool TryFire(float tickTime)
    {
        if (tickTime - lastFireTime >= 1f / FireRate)
        {
            lastFireTime = tickTime;
            return true;
        }
        return false;
    }

    public void Fire()
    {
        CreateProjectiles();
    }
    void CreateProjectiles()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            Vector3 offset = GetOffset(i);
            CreateProjectile(offset);
        }
    }
    Projectile CreateProjectile(Vector3 offset)
    {
        var projectile = GetFromPool();
        projectile.Die = ReturnToPool;
        projectile.Initialize(this, offset);
        return projectile;
    }

    Vector3 GetOffset(int index)
    {
        int frontRowCapacity = 3;
        float spacingX = 0.5f;
        float spacingY = -0.5f;

        if (index < frontRowCapacity)
        {
            int innerRowCount = Mathf.Min(ProjectileCount, frontRowCapacity);
            float x = (index - (innerRowCount - 1) / 2f) * spacingX;
            return new Vector3(x, 0, 0);
        }

        int rearIndex = index - frontRowCapacity;
        int col = rearIndex % 3;
        int row = rearIndex / 3 + 1;

        int rowCount = Mathf.Min(ProjectileCount - frontRowCapacity - 3 * (row - 1), 3);
        float xOffset = (col - (rowCount - 1) / 2f) * spacingX;
        float yOffset = row * spacingY;

        return new Vector3(xOffset, yOffset, 0);
    }

}
