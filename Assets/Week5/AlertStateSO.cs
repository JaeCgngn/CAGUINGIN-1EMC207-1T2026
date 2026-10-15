
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Alert")]
public class AlertStateSO : EnemyStateSO
{
    [Header("Alert Settings")]
    [SerializeField] private float m_AlertRadius = 4f;
    [SerializeField] private float m_AlertDuration = 2f;

    public override void Enter(EnemyBrain brain)
    {
        brain.ResetAlertTimer();
    }

    public override void Tick(EnemyBrain brain)
    {
        if (brain.Target == null)
        {
            brain.Patrol();
            return;
        }

        float distance = Vector3.Distance(
            brain.transform.position,
            brain.Target.position);

        if (distance > m_AlertRadius)
        {
            brain.Patrol();
            return;
        }

        brain.IncreaseAlertTimer(Time.deltaTime);

        if (brain.AlertTimer >= m_AlertDuration)
        {
            brain.Attack();
        }
    }

    public override void Exit(EnemyBrain brain) { }
}
