using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    abstract public void PerformAction();
    internal Vector3 GetLocation(GameObject gameObject)
    {
        return gameObject.transform.position;
    }
    internal void SetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        this.navMeshAgent = navMeshAgent;
    }

    internal NavMeshAgent GetNavMeshAgent() {
        return this.navMeshAgent;
    }

    internal void GoToLocation(Vector3 position) {
        navMeshAgent.SetDestination(position);
    }

}
