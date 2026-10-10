using UnityEngine;

[CreateAssetMenu(fileName = "EnemyState", menuName = "Week5/EnemyState")]
public class EnemyState : ScriptableObject
{
    public enum State
    {
        Patrol,
        Alert,
        Chase,
    }

    
}
