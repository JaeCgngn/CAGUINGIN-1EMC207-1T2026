
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

    public EnemyPatrol PatrolSettings => m_PatrolSettings;

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
}
