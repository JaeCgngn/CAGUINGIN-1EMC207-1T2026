
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    [Header("State Configuration")]
    [SerializeField] private EnemyStateSO m_PatrolState;
    [SerializeField] private EnemyStateSO m_AlertState;
    [SerializeField] private EnemyStateSO m_ChaseState;
    [SerializeField] private EnemyStateSO m_SearchState;

    [Header("Target")]
    [SerializeField] private Transform m_Target;

    [Header("Movement")]
    [SerializeField] private NavMeshAgent m_Agent;

    [Header("Patrol")]
    [SerializeField] private EnemyPatrol m_PatrolSettings;

    [Header("Vision")]
    [SerializeField] private EnemyVision m_Vision;

    private EnemyPatrolRoute m_PatrolRoute;
    private EnemyStateSO m_CurrentState;

    private Vector3 m_InvestigationPosition;
    private bool m_HasInvestigationPosition;

    private Vector3 m_LastKnownPosition;

    private float m_AlertTimer;
    private float m_SearchTimer;
    private float m_DefaultAgentSpeed;

    public Transform Target => m_Target;
    public NavMeshAgent Agent => m_Agent;
    public EnemyPatrol PatrolSettings => m_PatrolSettings;
    public EnemyVision Vision => m_Vision;

    public EnemyStateSO CurrentState => m_CurrentState;
    public EnemyStateSO AlertState => m_AlertState;
    public EnemyStateSO ChaseState => m_ChaseState;
    public EnemyStateSO SearchState => m_SearchState;

    public float AlertTimer => m_AlertTimer;
    public float SearchTimer => m_SearchTimer;

    public Vector3 LastKnownPosition => m_LastKnownPosition;
    public Vector3 InvestigationPosition => m_InvestigationPosition;
    public bool HasInvestigationPosition => m_HasInvestigationPosition;

    public event Action<EnemyStateSO> StateChanged;

    private void Start()
    {
        if (m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();

        if (m_PatrolSettings == null)
            m_PatrolSettings = GetComponent<EnemyPatrol>();

        if (m_Vision == null)
            m_Vision = GetComponent<EnemyVision>();



        m_PatrolRoute = FindFirstObjectByType<EnemyPatrolRoute>();

        if (m_Agent == null ||
     m_PatrolSettings == null ||
     m_PatrolRoute == null ||
     m_Vision == null)
        {
            Debug.LogError(
                $"{name}: Missing NavMeshAgent, EnemyPatrol, " +
                "EnemyPatrolRoute, or EnemyVision.",
                this);

            enabled = false;
            return;
        }

        if (m_Target == null && !TryAssignPlayerTarget())
        {
            Debug.LogError(
                $"{name}: No player found. Ensure the player has the Player tag.",
                this);

            enabled = false;
            return;
        }

        m_PatrolSettings.SetWaypoints(m_PatrolRoute.Waypoints);

        if (!m_PatrolSettings.HasWaypoints)
        {
            Debug.LogError(
                $"{name}: Patrol route has no waypoints.",
                this);

            enabled = false;
            return;
        }

        m_DefaultAgentSpeed = m_Agent.speed;

        ChangeState(m_PatrolState);
    }

    private bool TryAssignPlayerTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return false;

        m_Target = player.transform;
        return true;
    }

    private void Update()
    {
        m_CurrentState?.Tick(this);
    }

    public void ChangeState(EnemyStateSO newState)
    {
        if (newState == null || newState == m_CurrentState)
            return;

        m_CurrentState?.Exit(this);

        m_CurrentState = newState;
        m_CurrentState.Enter(this);

        StateChanged?.Invoke(m_CurrentState);
    }

    public bool IsTargetInFOV(Transform target)
    {
        return m_Vision != null && m_Vision.CanSee(target);
    }

    public void UpdateLastKnownPosition()
    {
        if (m_Target != null)
            m_LastKnownPosition = m_Target.position;
    }

    public void SetInvestigationPosition(Vector3 position)
    {
        m_InvestigationPosition = position;
        m_HasInvestigationPosition = true;
    }

    public void ResetAlertTimer()
    {
        m_AlertTimer = 0f;
    }

    public void IncreaseAlertTimer(float deltaTime)
    {
        m_AlertTimer += deltaTime;
    }

    public void ResetSearchTimer()
    {
        m_SearchTimer = 0f;
    }

    public void IncreaseSearchTimer(float deltaTime)
    {
        m_SearchTimer += deltaTime;
    }

    public void RestoreAgentSpeed()
    {
        if (m_Agent != null)
            m_Agent.speed = m_DefaultAgentSpeed;
    }

    public void Patrol()
    {
        ChangeState(m_PatrolState);
    }

    // Use when the enemy hears a noise at a known world position.
    public void Alert(Vector3 noisePosition)
    {
        SetInvestigationPosition(noisePosition);
        ChangeState(m_AlertState);
    }

    // Retained for existing code that calls Alert() without a position.
    public void Alert()
    {
        ChangeState(m_AlertState);
    }

    public void Chase()
    {
        ChangeState(m_ChaseState);
    }

    public void Search()
    {
        ChangeState(m_SearchState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_LastKnownPosition, 0.25f);

        if (m_HasInvestigationPosition)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(m_InvestigationPosition, 0.25f);
        }
    }
}
