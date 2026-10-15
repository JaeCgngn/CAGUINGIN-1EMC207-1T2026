
using UnityEngine;

public abstract class EnemyStateSO : ScriptableObject
{
    public abstract void Enter(EnemyBrain brain);
    public abstract void Tick(EnemyBrain brain);
    public abstract void Exit(EnemyBrain brain);
}
