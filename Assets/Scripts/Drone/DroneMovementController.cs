using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneMovementController : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    List<GameObject> patrolPath;


    int currentPoint;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent.SetDestination(getLocation(patrolPath[currentPoint]));
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!navMeshAgent.hasPath) {
            currentPoint = (currentPoint + 1) % patrolPath.Count;
            navMeshAgent.SetDestination(getLocation(patrolPath[currentPoint]));
        }
    }

    private Vector3 getLocation(GameObject gameObject)
    {
        return gameObject.transform.position;
    }

}
