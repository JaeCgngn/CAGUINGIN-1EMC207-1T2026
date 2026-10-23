using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0.1f)] private float m_MoveSpeed = 8f;
    [SerializeField, Min(0.1f)] private float m_Acceleration = 50f;
    [SerializeField, Min(0.1f)] private float m_StoppingDistance = 0.05f;

    [Header("Input")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private LayerMask m_GroundMask = ~0;

    private NavMeshAgent m_Agent;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        if (m_Camera == null)
            m_Camera = Camera.main;

        m_Agent.speed = m_MoveSpeed;
        m_Agent.acceleration = m_Acceleration;
        m_Agent.stoppingDistance = m_StoppingDistance;
        m_Agent.autoBraking = true;
    }

    private void Update()
    {
        if (Mouse.current == null ||
            !Mouse.current.rightButton.wasPressedThisFrame)
        {
            return;
        }

        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        if (m_Camera == null)
            return;

        Ray ray = m_Camera.ScreenPointToRay(
            Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, 500f, m_GroundMask))
            return;

        if (m_Agent.isOnNavMesh)
            m_Agent.SetDestination(hit.point);
    }
}