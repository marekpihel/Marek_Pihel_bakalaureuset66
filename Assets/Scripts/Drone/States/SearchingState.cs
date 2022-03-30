using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : State
{
    public override void PerformAction()
    {
        if (!GetNavMeshAgent().hasPath) {
            GoToLocation(new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
        }
    }
}
