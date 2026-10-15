
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AttackStateSO : EnemyStateSO
{
    [Header("Attack Settings")]
    [SerializeField] private float m_AttackRange = 2f;
    [SerializeField] private float m_AttackDamage = 10f;
    [SerializeField] private float m_AttackCooldown = 1.5f;

    public override void Enter(EnemyBrain brain)
    {
        brain.Agent.isStopped = true;
    }

    public override void Tick(EnemyBrain brain)
    {
        Transform target = brain.Target;

        if (target == null)
        {
            brain.Patrol();
            return;
        }

        float distance = Vector3.Distance(
            brain.transform.position,
            target.position);

        if (distance > m_AttackRange)
        {
            brain.Patrol();
            return;
        }

        if (brain.CanAttack())
        {
            Debug.Log("Enemy attacks the player for " + m_AttackDamage);
            brain.SetAttackCooldown(m_AttackCooldown);
        }
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.Agent.isStopped = false;
    }
}
