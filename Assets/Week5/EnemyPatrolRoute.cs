
using UnityEngine;

public class EnemyPatrolRoute : MonoBehaviour
{
    [SerializeField] private Transform[] m_Waypoints;

    public Transform[] Waypoints => m_Waypoints;

    private void Awake()
    {
        if (m_Waypoints != null && m_Waypoints.Length > 0)
            return;

        int childCount = transform.childCount;

        m_Waypoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            m_Waypoints[i] = transform.GetChild(i);
        }
    }
}
