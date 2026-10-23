using UnityEngine;

public class PlayerCaught : MonoBehaviour
{
    private bool m_IsCaught;

    public void GetCaught()
    {
        if (m_IsCaught)
            return;

        m_IsCaught = true;

        Debug.Log("Player caught! Restarting level.");

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RestartLevel();
        }
        else
        {
            Debug.LogError("LevelManager instance not found.");
        }
    }
}