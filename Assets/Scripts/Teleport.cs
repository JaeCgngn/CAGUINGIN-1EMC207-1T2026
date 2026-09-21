using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Teleport : MonoBehaviour
{
    private NavMeshLink[] links;

    private void Awake()
    {
        links = GetComponents<NavMeshLink>();
    }

    private void Update()
    {
        NavMeshAgent[] agents =
            FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        foreach (NavMeshAgent agent in agents)
        {
            if (!agent.isOnOffMeshLink)
                continue;

            OffMeshLinkData linkData = agent.currentOffMeshLinkData;

            foreach (NavMeshLink link in links)
            {
                if (linkData.owner != link)
                    continue;

                agent.Warp(linkData.endPos);
                agent.CompleteOffMeshLink();

                break;
            }
        }
    }
}