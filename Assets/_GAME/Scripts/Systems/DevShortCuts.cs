using UnityEngine;
using UnityEngine.SceneManagement;

public class DevShortCuts : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Instance.LoadCurrentLevel();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            LevelManager.CurrentLevel++;
            LevelManager.Instance.LoadCurrentLevel();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            LevelManager.CurrentLevel--;
            LevelManager.CurrentLevel = Mathf.Max(0, LevelManager.CurrentLevel);
            LevelManager.Instance.LoadCurrentLevel();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameEventSystem.Instance.Trigger_TargetKilled();
        }
    }
}
