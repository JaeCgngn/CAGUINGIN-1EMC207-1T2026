
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Chase")]
public class ChaseStateSO : EnemyStateSO
{
    [Header("Chase Settings")]
    [SerializeField, Min(0f)]
    private float m_ChaseSpeed = 5f;

    [Header("Detection")]
    [SerializeField, Min(0.1f)]
    private float m_CatchDistance = 1.5f;

    public override void Enter(EnemyBrain brain)
    {
        NavMeshAgent agent = brain.Agent;

        agent.isStopped = false;
        agent.speed = m_ChaseSpeed;

        brain.UpdateLastKnownPosition();

        if (brain.Target != null)
        {
            agent.SetDestination(brain.Target.position);
        }
        else
        {
            agent.SetDestination(brain.LastKnownPosition);
        }
    }

    public override void Tick(EnemyBrain brain)
    {
        NavMeshAgent agent = brain.Agent;
        Transform target = brain.Target;

        // The enemy lost sight of the player.
        if (!brain.IsTargetInFOV(target))
        {
            brain.Search();
            return;
        }
        // Check whether the player has been caught.
        PlayerCaught playerCaught =
            target.GetComponentInParent<PlayerCaught>();

        if (playerCaught != null)
        {
            float distance = Vector3.Distance(
                brain.transform.position,
                target.position);

            if (distance <= m_CatchDistance)
            {
                playerCaught.GetCaught();
                return;
            }
        }

        // The enemy lost sight of the player.
        if (!brain.IsTargetInFOV(target))
        {
            brain.Alert();
            return;
        }

        // Keep pursuing the visible player.
        brain.UpdateLastKnownPosition();
        agent.SetDestination(target.position);
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.Agent.ResetPath();
        brain.RestoreAgentSpeed();
    }
}
