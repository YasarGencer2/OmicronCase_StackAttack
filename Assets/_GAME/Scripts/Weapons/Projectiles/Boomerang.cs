using UnityEngine;

public class Boomerang : Projectile
{
    Transform playerTransform => GameHelper.Instance.PTransform;
    float curveDir;
    float rotationSpeed = 6f;
    float startTime; 
    float speed;
    Vector3 currentDir;
    public override void Initialize(Weapon weapon, Vector3 offset)
    {
        autoKill = false;
        base.Initialize(weapon, offset);

        curveDir = Random.value > 0.5f ? 1f : -1f;
        startTime = 0;
        currentDir = Vector3.up;
        speed = Weapon.Speed;
    }

    protected override void Move()
    {
        startTime += Time.deltaTime;
        float t = startTime / totalAliveTime;
        Vector3 targetDir;

        if (t <= 0.25f)
            targetDir = Vector3.right * curveDir * 0.7f + Vector3.up;
        else if (t <= 0.5f)
            targetDir = Vector3.right * -curveDir * 0.7f + Vector3.up;
        else if (t <= 0.75f)
            targetDir = Vector3.right * -curveDir * 0.7f - Vector3.up;
        else
            targetDir = (playerTransform.position - transform.position).normalized;

        var smoother = 4;
        if (t > .85f)
            smoother = 100;

        currentDir = Vector3.Lerp(currentDir, targetDir.normalized, Time.deltaTime * smoother);
        transform.position += currentDir * speed * Time.deltaTime;
        transform.Rotate(Vector3.forward, curveDir * rotationSpeed);

        if (t > .5f)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
            {
                Die(this);
            }
        }
    }
}
