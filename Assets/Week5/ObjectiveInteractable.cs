
using UnityEngine;

public class ObjectiveInteractable : MonoBehaviour, IInteractable
{
    [Header("Objective")]
    [SerializeField] private string m_ObjectiveName = "Identity Terminal";
    [SerializeField] private bool m_IsRequired = true;

    [Header("Interaction")]
    [SerializeField, Min(0.1f)]
    private float m_InteractionDuration = 5f;

    [Header("Noise")]
    [SerializeField] private float m_NoiseInterval = 2f;
    [SerializeField] private float m_NoiseRadius = 10f;

    private float m_Progress;
    private float m_NoiseTimer;
    private bool m_IsComplete;

    public string ObjectiveName => m_ObjectiveName;
    public float Progress => m_Progress / m_InteractionDuration;
    public bool IsComplete => m_IsComplete;

    private void Start()
    {
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogError("ObjectiveManager is missing.");
            return;
        }

        ObjectiveManager.Instance.RegisterObjective(m_IsRequired);
    }

    public void Interact()
    {
        // PlayerInteractor will call this when E is held.
        // Progress is handled in TickInteraction.
    }

    public void TickInteraction(bool isInteracting)
    {
        if (m_IsComplete)
            return;

        if (!isInteracting)
        {
            m_Progress = 0f;
            m_NoiseTimer = 0f;
            return;
        }

        m_Progress += Time.deltaTime;
        m_NoiseTimer += Time.deltaTime;

        if (m_NoiseTimer >= m_NoiseInterval)
        {
            m_NoiseTimer = 0f;
            EmitNoise();
        }

        if (m_Progress >= m_InteractionDuration)
            CompleteObjective();
    }

    private void EmitNoise()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            m_NoiseRadius,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);

        foreach (Collider hit in hits)
        {
            EnemyHearing hearing =
                hit.GetComponentInParent<EnemyHearing>();

            if (hearing != null)
                hearing.HearNoise(transform.position);
        }
    }

    private void CompleteObjective()
    {
        m_IsComplete = true;

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.CompleteObjective(m_IsRequired);

        Debug.Log($"{m_ObjectiveName} completed!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_NoiseRadius);
    }
}
