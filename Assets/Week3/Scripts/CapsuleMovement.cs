using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CapsuleMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = -1;

    public float normalSpeed = 5f;
    public float waterSpeed = 2f;

    private int waterMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        waterMask = 1 << NavMesh.GetAreaFromName("Water");
    }

    void Update()
    {
        //if(waypoints.Length = 0 || agent.pathPending )

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        { 
            MoveToRandomWaypoint();
        }

        NavMeshHit hit;
        if (agent.SamplePathPosition(NavMesh.AllAreas, 0.5f, out hit))
        {
            if ((hit.mask & waterMask) !=0)
            {
                agent.speed = waterSpeed;
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
