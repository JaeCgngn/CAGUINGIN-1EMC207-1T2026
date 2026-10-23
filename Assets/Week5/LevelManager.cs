using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private bool m_IsRestarting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RestartLevel()
    {
        if (m_IsRestarting)
            return;

        m_IsRestarting = true;

        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }
}