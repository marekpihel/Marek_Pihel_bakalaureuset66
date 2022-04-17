using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{

    List<GameObject> patrolPath;

    int currentPoint;

    public PatrolState(string stateName)
    {
        base.SetName(stateName);
    }

    public void Awake()
    {
        GoToLocation(GetLocation(patrolPath[currentPoint]));
        currentPoint = 0;
    }

    public override void PerformAction()
    {

        if (!GetNavMeshAgent().hasPath)
        {
            currentPoint = (currentPoint + 1) % patrolPath.Count;

            GoToLocation(GetLocation(patrolPath[currentPoint]));
        }
        if (patrolPath.Count == 1) {
            if (Vector3.Distance(GetNavMeshAgent().transform.position, patrolPath[0].transform.position) < 1) {
                GetNavMeshAgent().transform.rotation = patrolPath[0].transform.rotation;
            }
        }
    }

    #region Setters
    public void SetPatrolPath(List<GameObject> patrolPath)
    {
        this.patrolPath = patrolPath;
    }
    #endregion
}
