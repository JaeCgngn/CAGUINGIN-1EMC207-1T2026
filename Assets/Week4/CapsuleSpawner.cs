// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.AI;

// public class CapsuleSpawner : MonoBehaviour
// {
//     public GameObject groundAgentPrefab;
//     public Transform spawnPoint;
//     public Transform[] patrolWaypoints;


//     public int totalAgentsToSpawn = 100;
//     public float spawnInterval = 0.5f;






//     public float maxNavMeshSearchDistance = 5.0f;



//     private void Start()
//     {
//         StartCoroutine(SpawnAgentsRoutine());
//     }


//     private IEnumerator SpawnAgentsRoutine()
//     {
//         for (int i = 0; i < totalAgentsToSpawn; i++)
//         {
//             Vector3 spawnPos = GetValidNavMeshPosition(spawnPoint.position);
//             GameObject agentObj = Instantiate(groundAgentPrefab, spawnPos, spawnPoint.rotation);

//             // Get component directly from the instantiated prefab
//             if (agentObj.TryGetComponent<CapsulePatrol>(out var patrolComp))
//             {
//                 patrolComp.InitializeWaypoints(patrolWaypoints);
//             }

//             yield return new WaitForSeconds(spawnInterval);
//         }
//     }

//     private Vector3 GetValidNavMeshPosition(Vector3 sourcePosition)
//     {
//         if (NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, maxNavMeshSearchDistance, NavMesh.AllAreas))
//         {
//             return hit.position;
//         }
//         return sourcePosition;
//     }
// }