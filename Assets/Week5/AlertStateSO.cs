
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Alert")]
public class AlertStateSO : EnemyStateSO
{
    [Header("Investigation Settings")]
    [SerializeField, Min(0f)]
    private float m_InvestigationDuration = 4f;

    [SerializeField, Min(0.1f)]
    private float m_InvestigationStoppingDistance = 1f;

    public override void Enter(EnemyBrain brain)
    {
        brain.ResetAlertTimer();

        NavMeshAgent agent = brain.Agent;
        agent.isStopped = false;

        // Use the noise position when available.
        // Fall back to the last known player position for compatibility.
        Vector3 destination = brain.HasInvestigationPosition
            ? brain.InvestigationPosition
            : brain.LastKnownPosition;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                $"{brain.name}: Enemy is not on the NavMesh.", brain);
            return;
        }

        if (!agent.SetDestination(destination))
        {
            Debug.LogWarning(
                $"{brain.name}: Failed to set investigation destination: " +
                $"{destination}", brain);
        }
        else
        {
            Debug.Log(
                $"{brain.name}: Investigating noise at {destination}", brain);
        }
    }

    public override void Tick(EnemyBrain brain)
    {
        NavMeshAgent agent = brain.Agent;

        // Immediately chase when the player is seen.
        if (brain.IsTargetInFOV(brain.Target))
        {
            brain.UpdateLastKnownPosition();
            brain.Chase();
            return;
        }

        // Wait until the navigation system finishes calculating its path.
        if (agent.pathPending)
            return;

        float stoppingDistance = Mathf.Max(
            m_InvestigationStoppingDistance,
            agent.stoppingDistance);

        bool reachedDestination;

        if (agent.hasPath)
        {
            reachedDestination =
                agent.remainingDistance <= stoppingDistance;
        }
        else
        {
            // Handles cases where the agent is already at the destination.
            Vector3 offset = agent.destination - agent.transform.position;
            offset.y = 0f;

            reachedDestination =
                offset.sqrMagnitude <= stoppingDistance * stoppingDistance;
        }

        if (!reachedDestination)
            return;

        // The enemy has reached the noise location.
        // Stop and investigate for a short duration.
        agent.isStopped = true;

        brain.IncreaseAlertTimer(Time.deltaTime);

        if (brain.AlertTimer >= m_InvestigationDuration)
        {
            brain.Patrol();
        }
    }

    public override void Exit(EnemyBrain brain)
    {
        NavMeshAgent agent = brain.Agent;

        agent.isStopped = false;
        agent.ResetPath();
    }
}
