
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
            return;

        float distance = Vector3.Distance(
            transform.position, noisePosition);

        if (distance > m_HearingRadius)
            return;

        m_Brain.Alert(noisePosition);

        Debug.Log($"{name} heard a noise at {noisePosition}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position, m_HearingRadius);
    }
}
