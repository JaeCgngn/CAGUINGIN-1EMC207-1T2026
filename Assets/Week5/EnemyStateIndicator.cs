
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyBrain m_Brain;
    [SerializeField] private Image m_Icon;

    [Header("State Icons")]
    [SerializeField] private Sprite m_AlertIcon;
    [SerializeField] private Sprite m_SearchIcon;
    [SerializeField] private Sprite m_ChaseIcon;

    private Camera m_Camera;

    private void Awake()
    {
        if (m_Brain == null)
            m_Brain = GetComponentInParent<EnemyBrain>();

        if (m_Icon == null)
            Debug.LogError($"{name}: Icon Image is not assigned.", this);

        if (m_Brain == null)
            Debug.LogError($"{name}: EnemyBrain was not found.", this);
    }

    private void OnEnable()
    {
        if (m_Brain == null)
            return;

        m_Brain.StateChanged += OnStateChanged;

        // Handle cases where the brain initialized before this UI.
        OnStateChanged(m_Brain.CurrentState);
    }

    private void OnDisable()
    {
        if (m_Brain != null)
            m_Brain.StateChanged -= OnStateChanged;
    }

    private void Start()
    {
        m_Camera = Camera.main;
    }


    private void LateUpdate()
    {
        if (m_Camera == null)
            return;

        // Match the camera's rotation, ignoring the enemy's rotation.
        transform.rotation = m_Camera.transform.rotation;
    }


    private void OnStateChanged(EnemyStateSO state)
    {
        if (m_Icon == null || m_Brain == null)
            return;

        Sprite icon = null;

        if (state == m_Brain.AlertState)
            icon = m_AlertIcon;
        else if (state == m_Brain.SearchState)
            icon = m_SearchIcon;
        else if (state == m_Brain.ChaseState)
            icon = m_ChaseIcon;

        m_Icon.sprite = icon;
        m_Icon.enabled = icon != null;
    }
}
