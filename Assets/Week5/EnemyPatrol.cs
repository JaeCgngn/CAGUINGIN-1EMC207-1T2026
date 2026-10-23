
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform[] m_Waypoints;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float m_ArrivalDistance = 0.5f;
    [SerializeField, Min(0f)] private float m_WaitTime = 2f;

    private int m_CurrentWaypointIndex;
    private float m_WaitTimer;
    private bool m_IsWaiting;

    public bool HasWaypoints =>
        m_Waypoints != null && m_Waypoints.Length > 0;

    public void SetWaypoints(Transform[] waypoints)
    {
        m_Waypoints = waypoints;
        m_CurrentWaypointIndex = 0;
        m_IsWaiting = false;
        m_WaitTimer = 0f;
    }

    public void BeginPatrol(NavMeshAgent agent)
    {
        if (!HasWaypoints || !agent.isOnNavMesh)
            return;

        m_CurrentWaypointIndex = 0;
        m_IsWaiting = false;
        m_WaitTimer = 0f;

        agent.isStopped = false;
        agent.SetDestination(CurrentWaypoint);
    }

    public void PatrolStep(NavMeshAgent agent)
    {
        if (!HasWaypoints || !agent.isOnNavMesh)
            return;

        if (m_IsWaiting)
        {
            m_WaitTimer -= Time.deltaTime;

            if (m_WaitTimer <= 0f)
            {
                m_IsWaiting = false;
                m_CurrentWaypointIndex =
                    (m_CurrentWaypointIndex + 1) % m_Waypoints.Length;

                agent.SetDestination(CurrentWaypoint);
            }

            return;
        }

        if (agent.pathPending)
            return;

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            return;

        if (agent.remainingDistance <= m_ArrivalDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f))
        {
            m_IsWaiting = true;
            m_WaitTimer = m_WaitTime;
        }
    }

    public Vector3 CurrentWaypoint =>
        m_Waypoints[m_CurrentWaypointIndex].position;
}
