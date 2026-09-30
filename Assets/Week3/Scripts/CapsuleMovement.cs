using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CapsuleMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = -1;

    public float normalSpeed = 10f;
    private float slowSpeed;

    private int slowArea;
    private int slowAreaMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        slowSpeed = normalSpeed * 0.5f;

        slowArea = NavMesh.GetAreaFromName("Water");
        slowAreaMask = 1 << slowArea;

        agent.speed = normalSpeed;

        agent.SetAreaCost(slowArea, 2f);
    }

    void Update()
    {
        //if(waypoints.Length = 0 || agent.pathPending )

        if (!agent.pathPending && agent.remainingDistance < 1f)
        { 
            MoveToRandomWaypoint();
        }

        UpdateSpeed();

    }

    public void UpdateSpeed()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            transform.position,
            out hit,
            1.0f,
            NavMesh.AllAreas))
        {
            if ((hit.mask & slowAreaMask) != 0)
            {
                agent.speed = slowSpeed;
            }
            else
            {
                agent.speed = normalSpeed;
            }
        }
    }

    public void MoveToRandomWaypoint()
    {
        if (waypoints.Length == 1)
        {
            agent.SetDestination(waypoints[0].position);
            return;
        }

        int newIndex = currentWaypointIndex;
        while (newIndex == currentWaypointIndex)
        {
            newIndex = Random.Range(0, waypoints.Length);
            
        }
        
        currentWaypointIndex = newIndex;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }


}
