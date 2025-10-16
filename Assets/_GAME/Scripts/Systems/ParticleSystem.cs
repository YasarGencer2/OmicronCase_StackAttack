using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
    public static ParticleSystem Instance { get; private set; }

    [SerializeField] ParticleData[] particles;
    Dictionary<ParticleType, Queue<UnityEngine.ParticleSystem>> pool = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var data in particles)
        {
            pool[data.type] = new Queue<UnityEngine.ParticleSystem>();
            if (data.particle != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    var p = Instantiate(data.particle, transform);
                    p.gameObject.SetActive(false);
                    pool[data.type].Enqueue(p);
                }
            }
        }
    }

    public UnityEngine.ParticleSystem Play(ParticleType type, Vector3 position)
    {
        if (!pool.ContainsKey(type))
            return null;

        UnityEngine.ParticleSystem particle;
        if (pool[type].Count > 0)
        {
            particle = pool[type].Dequeue();
            particle.transform.position = position;
            particle.gameObject.SetActive(true);
        }
        else
        {
            var data = System.Array.Find(particles, p => p.type == type);
            if (data.particle == null) return null;
            particle = Instantiate(data.particle, transform);
            particle.transform.position = position;
        }

        particle.Play();
        StartCoroutine(ReturnToPoolAfter(particle));
        return particle;
    }

    System.Collections.IEnumerator ReturnToPoolAfter(UnityEngine.ParticleSystem particle)
    {
        yield return new WaitForSeconds(particle.main.duration + particle.main.startLifetime.constantMax);
        particle.gameObject.SetActive(false);
        var type = System.Array.Find(particles, p => p.particle == particle || p.particle.name == particle.name.Replace("(Clone)","")).type;
        pool[type].Enqueue(particle);
    }
}

public enum ParticleType
{
    TargetKilled = 1,
}

[System.Serializable]
public struct ParticleData
{
    public ParticleType type;
    public UnityEngine.ParticleSystem particle;
}
