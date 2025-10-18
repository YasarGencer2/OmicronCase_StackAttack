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
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameHelper.Instance.PMovemet.Dash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 3f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = .5f;
        }
    }
}
