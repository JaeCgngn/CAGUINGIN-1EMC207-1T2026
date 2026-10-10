using UnityEngine;
using UnityEngine.AI;

public class W5EnemyMove : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;


    [SerializeField] public Transform[] patrolpoints;
    private int currentpoint = 0;

    private EnemyState enemyState;

    public float speed = 3f;


    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        
    }

    public void StatePatrol()
    {
        
    }

    private void MoveToNextWaypoint()
    {
        currentpoint = (currentpoint + 1) % patrolpoints.Length;
        agent.SetDestination(patrolpoints[currentpoint].position);
    }
}
