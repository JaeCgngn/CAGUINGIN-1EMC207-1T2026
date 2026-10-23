
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private Transform m_Eyes;
    [SerializeField, Min(0f)] private float m_DetectionRadius = 10f;
    [SerializeField, Range(0f, 360f)] private float m_FieldOfView = 90f;
    [SerializeField] private LayerMask m_ObstacleMask;

    public float DetectionRadius => m_DetectionRadius;
    public float FieldOfView => m_FieldOfView;

    private Vector3 EyePosition =>
        m_Eyes != null ? m_Eyes.position : transform.position + Vector3.up;

    public bool CanSee(Transform target)
    {
        if (target == null)
            return false;

        Vector3 origin = EyePosition;
        Vector3 toTarget = target.position - origin;
        float distance = toTarget.magnitude;

        if (distance > m_DetectionRadius)
            return false;

        Vector3 direction =
            Vector3.ProjectOnPlane(toTarget, Vector3.up);

        Vector3 forward =
            Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        if (direction.sqrMagnitude > Mathf.Epsilon &&
            forward.sqrMagnitude > Mathf.Epsilon)
        {
            float angle = Vector3.Angle(forward, direction);

            if (angle > m_FieldOfView * 0.5f)
                return false;
        }

        if (m_ObstacleMask.value != 0 &&
            distance > 0f &&
            Physics.Raycast(
                origin,
                toTarget.normalized,
                distance,
                m_ObstacleMask,
                QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 forward =
            Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        if (forward.sqrMagnitude < Mathf.Epsilon)
            return;

        forward.Normalize();

        Vector3 origin = EyePosition;
        float halfFOV = m_FieldOfView * 0.5f;
        const int segments = 32;

        Gizmos.color = Color.yellow;

        Vector3 previousPoint = origin +
            Quaternion.AngleAxis(-halfFOV, Vector3.up) *
            forward * m_DetectionRadius;

        Gizmos.DrawLine(origin, previousPoint);

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Lerp(
                -halfFOV,
                halfFOV,
                i / (float)segments);

            Vector3 point = origin +
                Quaternion.AngleAxis(angle, Vector3.up) *
                forward * m_DetectionRadius;

            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }

        Vector3 rightBoundary =
            Quaternion.AngleAxis(halfFOV, Vector3.up) * forward;

        Gizmos.DrawLine(
            origin,
            origin + rightBoundary * m_DetectionRadius);
    }
}
