
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Patrol")]
public class PatrolStateSO : EnemyStateSO
{
    // [Header("Detection")]
    // [SerializeField] private float m_DetectionRadius = 8f;
    //[SerializeField, Range(0f, 360f)] private float m_FieldOfView = 90f;

    //public float DetectionRadius => m_DetectionRadius;
    //public float FieldOfView => m_FieldOfView;

    public override void Enter(EnemyBrain brain)
    {
        brain.PatrolSettings.BeginPatrol(brain.Agent);
    }

    public override void Tick(EnemyBrain brain)
    {
        if (brain.IsTargetInFOV(brain.Target))
        {
            brain.UpdateLastKnownPosition();
            brain.Alert();
            return;
        }

        brain.PatrolSettings.PatrolStep(brain.Agent);
    }

    // private bool IsWithinFieldOfView(Transform observer, Vector3 toTarget)
    // {
    //     Vector3 direction = Vector3.ProjectOnPlane(toTarget, Vector3.up);
    //     Vector3 forward = Vector3.ProjectOnPlane(observer.forward, Vector3.up);

    //     if (direction.sqrMagnitude < Mathf.Epsilon || forward.sqrMagnitude < Mathf.Epsilon)
    //         return true;

    //     return Vector3.Angle(forward, direction) <= m_FieldOfView * 0.5f;
    // }

    public override void Exit(EnemyBrain brain)
    {
        brain.Agent.ResetPath();
    }
}
