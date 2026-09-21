using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 2f;

    [Header("Direction")]
    [SerializeField] private Vector3 direction = Vector3.right;

    [Header("Pause")]
    [SerializeField] private float pauseTime = 0f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float pauseTimer;
    private bool movingToEnd = true;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + direction.normalized * distance;
    }

    private void Update()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        Vector3 target = movingToEnd ? endPosition : startPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingToEnd = !movingToEnd;
            pauseTimer = pauseTime;
        }
    }
}