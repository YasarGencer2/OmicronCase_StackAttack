using UnityEngine;
using UnityEngine.SceneManagement;

public class DevShortCuts : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
