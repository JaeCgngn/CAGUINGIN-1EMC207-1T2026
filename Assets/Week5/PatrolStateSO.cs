
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Patrol")]
public class PatrolStateSO : EnemyStateSO
{
    [Header("Detection")]
    [SerializeField] private float m_DetectionRadius = 8f;

    public override void Enter(EnemyBrain brain)
    {
        brain.PatrolSettings.BeginPatrol(brain.Agent);
    }

    public override void Tick(EnemyBrain brain)
    {
        if (brain.Target != null)
        {
            float distance = Vector3.Distance(
                brain.transform.position,
                brain.Target.position);

            if (distance <= m_DetectionRadius)
            {
                brain.Alert();
                return;
            }
        }

        brain.PatrolSettings.PatrolStep(brain.Agent);
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.Agent.ResetPath();
    }
}
