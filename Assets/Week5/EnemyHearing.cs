
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    [Header("Hearing")]
    [SerializeField, Min(0f)]
    private float m_HearingRadius = 10f;

    [SerializeField]
    private EnemyBrain m_Brain;

    private void Awake()
    {
        if (m_Brain == null)
            m_Brain = GetComponent<EnemyBrain>();
    }


    public void HearNoise(Vector3 noisePosition)
    {
        if (m_Brain == null)
        {
            Debug.LogError(
                $"{name}: EnemyBrain reference is missing.", this);
            return;
        }

        float distance = Vector3.Distance(
            transform.position, noisePosition);

        Debug.Log(
            $"{name}: Noise received. Distance: {distance:F2}, " +
            $"Hearing radius: {m_HearingRadius}", this);

        if (distance > m_HearingRadius)
        {
            Debug.Log($"{name}: Noise is out of hearing range.", this);
            return;
        }

        m_Brain.Alert(noisePosition);

        Debug.Log(
            $"{name}: Alert requested. Current state: " +
            $"{m_Brain.CurrentState?.name}", this);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position, m_HearingRadius);
    }
}
