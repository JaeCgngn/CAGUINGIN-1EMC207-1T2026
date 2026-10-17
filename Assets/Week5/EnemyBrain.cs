
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    [Header("State Configuration")]
    [SerializeField] private EnemyStateSO m_PatrolState;
    [SerializeField] private EnemyStateSO m_AlertState;
    [SerializeField] private EnemyStateSO m_AttackState;

    [Header("Target")]
    [SerializeField] private Transform m_Target;

    [Header("Movement")]
    [SerializeField] private NavMeshAgent m_Agent;

    [Header("Patrol")]
    [SerializeField] private EnemyPatrol m_PatrolSettings;

    [Header("FOV")]
    [SerializeField, Range(0f, 360f)] private float m_FieldOfView = 90f;
    [SerializeField] private float m_DetectionRadius = 10f;
    [SerializeField] private LayerMask m_ObstacleMask;
    
    
        public float FieldOfView => m_FieldOfView;
    public EnemyPatrol PatrolSettings => m_PatrolSettings;
    public float DetectionRadius => m_DetectionRadius;


    public NavMeshAgent Agent => m_Agent;
    private EnemyStateSO m_CurrentState;
    private float m_AlertTimer;
    public Transform Target => m_Target;
    public float AlertTimer => m_AlertTimer;
    private float m_NextAttackTime;




    private void Start()
    {
        ChangeState(m_PatrolState);
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
    }
    public void ResetAlertTimer()
    {
        m_AlertTimer = 0f;
    }

    public void IncreaseAlertTimer(float deltaTime)
    {
        m_AlertTimer += deltaTime;
    }

    public bool CanAttack()
    {
        return Time.time >= m_NextAttackTime;
    }

    public void SetAttackCooldown(float cooldown)
    {
        m_NextAttackTime = Time.time + cooldown;
    }

    public void Patrol() => ChangeState(m_PatrolState);
    public void Alert() => ChangeState(m_AlertState);
    public void Attack() => ChangeState(m_AttackState);


    

    public bool IsTargetInFOV(Transform target)
    {
        if (target == null)
            return false;

        Vector3 toTarget = target.position - transform.position;

        if (toTarget.sqrMagnitude > m_DetectionRadius * m_DetectionRadius)
            return false;

        // Angle FOV
        Vector3 direction = Vector3.ProjectOnPlane(toTarget, Vector3.up);
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        if (direction.sqrMagnitude < Mathf.Epsilon || forward.sqrMagnitude < Mathf.Epsilon)
            return true;

        if (Vector3.Angle(forward, direction) > m_FieldOfView * 0.5f)
            return false;

        // Obstacle check
        if (m_ObstacleMask != 0)
        {
            float distance = Mathf.Sqrt(toTarget.sqrMagnitude);
            if (Physics.Raycast(transform.position + Vector3.up, toTarget.normalized, distance, m_ObstacleMask))
                return false; 
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        if (forward.sqrMagnitude < Mathf.Epsilon)
            return;

        forward.Normalize();
        Vector3 origin = transform.position;
        float halfFOV = m_FieldOfView * 0.5f;
        const int segments = 32;

        Gizmos.color = Color.yellow;

        // Left Boundary Line
        Vector3 previousPoint = origin + Quaternion.AngleAxis(-halfFOV, Vector3.up) * forward * m_DetectionRadius;
        Gizmos.DrawLine(origin, previousPoint);

        // Arc Perimeter
        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-halfFOV, halfFOV, i / (float)segments);
            Vector3 point = origin + Quaternion.AngleAxis(angle, Vector3.up) * forward * m_DetectionRadius;

            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }

        // Right Boundary Line
        Vector3 endDirection = Quaternion.AngleAxis(halfFOV, Vector3.up) * forward;
        Gizmos.DrawLine(origin, origin + endDirection * m_DetectionRadius);
    }
}
