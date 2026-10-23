
using UnityEngine;
using TMPro;

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

    [Header("World Space Prompt")]
    [SerializeField] private GameObject m_PromptRoot;
    [SerializeField] private TMP_Text m_PromptText;


    private float m_Progress;
    private float m_NoiseTimer;
    private bool m_IsComplete;

    public string ObjectiveName => m_ObjectiveName;
    public float Progress => m_Progress / m_InteractionDuration;
    public bool IsComplete => m_IsComplete;

    private void Awake()
    {
        if (m_PromptRoot != null)
            m_PromptRoot.SetActive(false);
    }

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
        EnemyHearing[] enemies =
            FindObjectsByType<EnemyHearing>(
                FindObjectsSortMode.None);

        foreach (EnemyHearing enemy in enemies)
        {
            if (enemy == null || !enemy.isActiveAndEnabled)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position);

            Debug.Log(
                $"Noise check: {enemy.name}, " +
                $"Distance: {distance:F1}, " +
                $"Objective radius: {m_NoiseRadius}");

            if (distance <= m_NoiseRadius)
            {
                enemy.HearNoise(transform.position);
            }
        }
    }


    private void CompleteObjective()
    {
        m_IsComplete = true;

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.CompleteObjective(m_IsRequired);

        Debug.Log($"{m_ObjectiveName} completed!");
    }

    public void SetPromptVisible(bool visible)
    {
        if (m_PromptRoot != null)
            m_PromptRoot.SetActive(visible && !m_IsComplete);
    }

    public void UpdatePrompt(bool isInteracting)
    {
        if (m_PromptText == null)
            return;

        if (m_IsComplete)
        {
            m_PromptText.text = $"{m_ObjectiveName} - Complete";
            return;
        }

        m_PromptText.text = isInteracting
            ? $"Working... {Progress * 100f:0}%"
            : $"Hold E - {m_ObjectiveName}";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_NoiseRadius);
    }
}
