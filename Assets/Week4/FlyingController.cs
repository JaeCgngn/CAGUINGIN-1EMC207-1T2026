using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class FlyingController : MonoBehaviour
{
    [Header("Patrol Routes")]
    public Transform[] groundRouteWaypoints;
    public Transform[] aerialRouteWaypoints;

    [Header("Flight")]
    public float targetHoverHeight = 4.0f;
    public float heightSmoothSpeed = 3.0f;
    public float noiseFrequency = 0.5f;
    public float noiseAmplitude = 0.8f;

    [Header("Slowdown")]
    public float baseSpeed = 8.0f;
    public float minVerticalSpeedMultiplier = 0.35f;
    public float verticalAngleThreshold = 15.0f;

    private NavMeshAgent navAgent;
    private Transform[] currentRoute;
    private int currentWaypointIndex = 0;
    private bool isAerialRoute = true;
    private float currentHeightOffset;
    private Vector3 previousPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = baseSpeed;
        previousPosition = transform.position;

        
        SetRoute(aerialRouteWaypoints);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePatrolNavigation();
        SmoothingHeightAndSpeed();
    }

    private void HandlePatrolNavigation()
    {
        if (currentRoute == null || currentRoute.Length == 0) return;

        if (!navAgent.pathPending && navAgent.remainingDistance < 2f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= currentRoute.Length)
            {
                currentWaypointIndex = 0;
                isAerialRoute = !isAerialRoute;
                SetRoute(isAerialRoute ? aerialRouteWaypoints : groundRouteWaypoints);
            }
            else
            {
                navAgent.SetDestination(currentRoute[currentWaypointIndex].position);
            }
        }
    }

    private void SetRoute(Transform[] newRoute)
    {
        currentRoute = newRoute;
        if (currentRoute != null && currentRoute.Length > 0)
        {
            navAgent.SetDestination(currentRoute[currentWaypointIndex].position);
        }
    }

    private void SmoothingHeightAndSpeed()
    {
        Vector3 currentPos = transform.position;
        Vector3 movementDelta = currentPos - previousPosition;
        previousPosition = currentPos;

        float verticalDistance = movementDelta.y;
        float horizontalDistance = new Vector2(movementDelta.x, movementDelta.z).magnitude;

        if (horizontalDistance > 0.001f)
        {
            float pitchAngle = Mathf.Atan2(Mathf.Abs(verticalDistance), horizontalDistance) * Mathf.Rad2Deg;

            if (pitchAngle > verticalAngleThreshold)
            {
                float speedFactor = Mathf.Clamp01(1.0f - (pitchAngle / 90.0f));
                navAgent.speed = Mathf.Max(baseSpeed * speedFactor, baseSpeed * minVerticalSpeedMultiplier);
            }
            else
            {
                navAgent.speed = baseSpeed;
            }
        }

        float desiredY = currentPos.y;
        if (Physics.Raycast(currentPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 50f))
        {
            desiredY = hit.point.y + targetHoverHeight;
        }

        currentHeightOffset = Mathf.Lerp(currentHeightOffset, desiredY - navAgent.nextPosition.y, Time.deltaTime * heightSmoothSpeed);
        

        Vector3 targetVisualPos = navAgent.nextPosition;
        targetVisualPos.y += currentHeightOffset;
        transform.position = targetVisualPos;
    }
}
