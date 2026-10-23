
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Search")]
public class SearchStateSO : EnemyStateSO
{
    [Header("Search Settings")]
    [SerializeField, Min(0.1f)]
    private float m_SearchSpeed = 3.5f;

    [SerializeField, Min(0f)]
    private float m_SearchDuration = 5f;

    [SerializeField, Min(0.1f)]
    private float m_ArrivalDistance = 1f;

    public override void Enter(EnemyBrain brain)
    {
        NavMeshAgent agent = brain.Agent;

        agent.isStopped = false;
        agent.speed = m_SearchSpeed;

        brain.ResetSearchTimer();

        // Investigate where the player was last seen.
        agent.SetDestination(brain.LastKnownPosition);
    }

    public override void Tick(EnemyBrain brain)
    {
        NavMeshAgent agent = brain.Agent;
        Transform target = brain.Target;

        // If the player is visible again, resume the chase.
        if (target != null && brain.IsTargetInFOV(target))
        {
            brain.UpdateLastKnownPosition();
            brain.Chase();
            return;
        }

        // Wait until the enemy reaches the last known position.
        if (agent.pathPending)
            return;

        if (agent.remainingDistance > m_ArrivalDistance)
            return;

        // Search the area for a limited duration.
        brain.IncreaseSearchTimer(Time.deltaTime);

        if (brain.SearchTimer >= m_SearchDuration)
        {
            brain.Patrol();
        }
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.Agent.ResetPath();
        brain.RestoreAgentSpeed();
    }
}
