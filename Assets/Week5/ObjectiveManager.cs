
using System;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    public event Action ObjectivesChanged;

    private int m_CompletedObjectives;
    private int m_RequiredObjectives;

    public int CompletedObjectives => m_CompletedObjectives;
    public int RequiredObjectives => m_RequiredObjectives;

    public bool CanEscape =>
        m_CompletedObjectives >= m_RequiredObjectives;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple ObjectiveManagers found.");
            enabled = false;
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void RegisterObjective(bool isRequired)
    {
        if (!isRequired)
            return;

        m_RequiredObjectives++;
        ObjectivesChanged?.Invoke();
    }

    public void CompleteObjective(bool isRequired)
    {
        if (!isRequired)
            return;

        m_CompletedObjectives++;
        ObjectivesChanged?.Invoke();

        Debug.Log(
            $"Objectives: {m_CompletedObjectives}/{m_RequiredObjectives}");
    }
}
