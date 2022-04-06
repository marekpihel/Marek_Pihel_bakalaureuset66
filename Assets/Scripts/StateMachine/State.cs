using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool isFinished;
    string stateName;

    abstract public void PerformAction();
    public string GetName() { return stateName; }
    public void SetName(string stateName) { this.stateName = stateName; }

    public Vector3 GetLocation(GameObject gameObject)
    {
        return gameObject.transform.position;
    }
    public void SetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        this.navMeshAgent = navMeshAgent;
    }

    public NavMeshAgent GetNavMeshAgent() {
        return this.navMeshAgent;
    }

    public void SetIsFinished(bool isFinished) {
        this.isFinished = isFinished;
    }

    internal void GoToLocation(Vector3 position) {
        navMeshAgent.SetDestination(position);
    }

    internal bool GetIsFinished()
    {
        return this.isFinished;
    }
}
