
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyNoiseTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyBrain m_EnemyBrain;
    [SerializeField] private Transform m_NoiseSource;

    [Header("Testing")]
    [SerializeField] private Key m_TestKey = Key.N;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[m_TestKey].wasPressedThisFrame)
        {
            TriggerNoise();
        }
    }

    private void TriggerNoise()
    {
        if (m_EnemyBrain == null)
        {
            Debug.LogWarning(
                "EnemyNoiseTest: Enemy Brain is not assigned.");
            return;
        }

        Vector3 noisePosition = m_NoiseSource != null
            ? m_NoiseSource.position
            : transform.position;

        m_EnemyBrain.Alert(noisePosition);

        Debug.Log($"Noise triggered at {noisePosition}");
    }
}
