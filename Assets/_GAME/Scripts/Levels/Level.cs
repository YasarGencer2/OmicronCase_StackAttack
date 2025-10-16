using UnityEngine;

public class Level : MonoBehaviour
{
    public void ValidateAllTargets()
    {
        Target[] targets = GetComponentsInChildren<Target>(true);
        foreach (Target target in targets)
        {
            target.Start();
        }
    }
}