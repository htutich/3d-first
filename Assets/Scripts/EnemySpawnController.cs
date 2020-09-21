using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnController : MonoBehaviour
{

    public List<Transform> EnemyPoints = new List<Transform>();
    public GameObject Enemy;
    bool canSpawn = true;
    int maxCount = 4;

    [SerializeField]
    NavMeshSurface[] NavMeshSurfaces;

    private void OnTriggerEnter(Collider other)
    {

        if (canSpawn && maxCount > 0)
        {
            foreach (Transform tr in EnemyPoints)
            {
                Instantiate(Enemy, tr, false);
                maxCount--;
            }
        }

    }

    private void Start()
    {
        for (int i = 0; i < NavMeshSurfaces.Length; i++)
        {
            if (NavMeshSurfaces[i] == null) continue;
            NavMeshSurfaces[i].BuildNavMesh();
        }
    }
}
