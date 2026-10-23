
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField, Min(0.1f)]
    private float m_InteractionRange = 2f;

    private ObjectiveInteractable m_CurrentObjective;

    private void Update()
    {
        ObjectiveInteractable nearbyObjective =
            FindNearbyObjective();

        if (nearbyObjective != m_CurrentObjective)
        {
            SetPromptVisible(m_CurrentObjective, false);
            m_CurrentObjective = nearbyObjective;
            SetPromptVisible(m_CurrentObjective, true);
        }

        bool isInteracting =
            m_CurrentObjective != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.isPressed;

        if (m_CurrentObjective != null)
        {
            m_CurrentObjective.TickInteraction(isInteracting);
            m_CurrentObjective.UpdatePrompt(isInteracting);
        }
    }

    private ObjectiveInteractable FindNearbyObjective()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            m_InteractionRange,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);

        ObjectiveInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            ObjectiveInteractable objective =
                hit.GetComponentInParent<ObjectiveInteractable>();

            if (objective == null || objective.IsComplete)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                objective.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = objective;
            }
        }

        return closest;
    }

    private void SetPromptVisible(
        ObjectiveInteractable objective, bool visible)
    {
        if (objective != null)
            objective.SetPromptVisible(visible);
    }

    private void OnDisable()
    {
        SetPromptVisible(m_CurrentObjective, false);
        m_CurrentObjective = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position, m_InteractionRange);
    }
}
