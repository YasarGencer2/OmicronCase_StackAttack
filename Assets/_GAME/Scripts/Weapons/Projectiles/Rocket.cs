using UnityEngine;

public class Rocket : Projectile
{
    float angle;
    float targetAngle;
    Vector3 direction;
    bool exploded = false;
    [SerializeField] RocketExplosion explosionPrefab;
    RocketExplosion myExplosion;

    public override void Initialize(Weapon weapon, Vector3 offset)
    {
        autoKill = false;
        base.Initialize(weapon, offset);
        angle = Random.Range(10f, 25f) * (Random.value > 0.5f ? 1f : -1f);
        targetAngle = 0f;
        direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
        exploded = false;
        Invoke("Explode", totalAliveTime);
    }

    protected override void Move()
    {
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 0.5f);
        direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
        transform.position += direction * Weapon.Speed * Time.deltaTime;
        transform.up = direction;
    }
    protected override void InflictDamage(Target target)
    {
        base.InflictDamage(target);
        Explode();
    }
    void Explode()
    {
        if (exploded)
            return;
        exploded = true;
        if (myExplosion == null)
            myExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        myExplosion.transform.position = transform.position;
        myExplosion.SetDamage(Weapon.Damage);
        myExplosion.gameObject.SetActive(true);
        gameObject.SetActive(false);
        AutoDie(1);
    } 
}
