using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneMovementController : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    List<GameObject> patrolPath;

    StateMachine stateMachine;
    PatrolState patrolState;
    InvestigateState investigateState;
    LookAroundState lookAroundState;
    SearchingState searchingState;

    [SerializeField]
    bool search, investigate, lookaround, patrol, stateChanged;
    State previousStateName;









    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        patrolState = new PatrolState();
        investigateState = new InvestigateState();
        lookAroundState = new LookAroundState();
        searchingState = new SearchingState();
        patrolState.SetNavMeshAgent(navMeshAgent);
        searchingState.SetNavMeshAgent(navMeshAgent);
        patrolState.SetPatrolPath(patrolPath);
        stateMachine.SetCurrentState(patrolState);
        previousStateName = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasStateChanged(stateMachine.GetCurrentState())) {
            ChangeState();
        }
        stateMachine.GetCurrentState().PerformAction();
    }

    private bool HasStateChanged(State currentState)
    {
        if (!currentState == previousStateName) {
            stateChanged = true;
        }
        return stateChanged;
    }

    private void ChangeState()
    {
        stateChanged = false;
        if (search)
        {
            searchingState.GetNavMeshAgent().ResetPath();
            stateMachine.SetCurrentState(searchingState);
            previousStateName = searchingState;
        }
        else if (investigate)
        {
            stateMachine.SetCurrentState(investigateState);
            previousStateName = investigateState;
        }
        else if (lookaround)
        {
            stateMachine.SetCurrentState(lookAroundState);
            previousStateName = lookAroundState;
        }
        else if (patrol)
        {
            stateMachine.SetCurrentState(patrolState);
            previousStateName = patrolState;
        }
    }
}
