
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform m_Target;

    [Header("Camera Position")]
    [SerializeField]
    private Vector3 m_Offset =
        new Vector3(0f, 5f, -7f);

    [Header("Follow Settings")]
    [SerializeField, Min(0f)]
    private float m_FollowSmoothness = 5f;

    [SerializeField, Min(0f)]
    private float m_RotationSmoothness = 5f;

    private void LateUpdate()
    {
        if (m_Target == null)
            return;

        FollowTarget();
        RotateTowardsTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = m_Target.position + m_Offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            m_FollowSmoothness * Time.deltaTime
        );
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = m_Target.position - transform.position;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            m_RotationSmoothness * Time.deltaTime
        );
    }
}
