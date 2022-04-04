using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneMovementController : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    List<GameObject> patrolPath;

    bool search, investigate, patrol, stateChanged;
    Vector3 pointOfInterest;

    StateMachine stateMachine;
    PatrolState patrolState;
    InvestigateState investigateState;
    SearchingState searchingState;
   
    State previousState;

    #region Initializing
    private void InitializeStates()
    {
        patrolState = new PatrolState();
        investigateState = new InvestigateState();
        searchingState = new SearchingState();
    }

    private void InitializeNavMeshAgent()
    {
        patrolState.SetNavMeshAgent(navMeshAgent);
        searchingState.SetNavMeshAgent(navMeshAgent);
        investigateState.SetNavMeshAgent(navMeshAgent);
    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        InitializeStates();
        InitializeNavMeshAgent();
        patrolState.SetPatrolPath(patrolPath);
        stateMachine.SetCurrentState(patrolState);
        previousState = patrolState;
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.menuOpened) {
            stateMachine.GetCurrentState().GetNavMeshAgent().isStopped = false;
            if (!HasStateChanged(stateMachine.GetCurrentState())) {
                ChangeState();
            } 
            if (stateMachine.GetCurrentState().GetIsFinished()) { patrol = true; }
            stateMachine.GetCurrentState().PerformAction();
        } else {
            stateMachine.GetCurrentState().GetNavMeshAgent().isStopped = true;
        }
    }

    private bool HasStateChanged(State currentState)
    {
        if (!currentState == previousState) {
            stateChanged = true;
        }
        return stateChanged;
    }

    private void ChangeState()
    {
        previousState = stateMachine.GetCurrentState();
        if (search){
            searchingState.GetNavMeshAgent().ResetPath();
            searchingState.SetPointOfInterest(pointOfInterest);
            searchingState.SetSearchRadius(5);
            stateMachine.SetCurrentState(searchingState);
        } else if (investigate) {
            investigateState.GetNavMeshAgent().ResetPath();
            investigateState.SetPointOfInterest(pointOfInterest);
            investigateState.SetSearchRadius(5);
            investigateState.ResetSearchAmount();
            investigateState.SetIsFinished(false);
            stateMachine.SetCurrentState(investigateState);
        } else if (patrol) {
            stateMachine.SetCurrentState(patrolState);
        }
        ResetStateBooleans();
    }


    private void ResetStateBooleans()
    {
        patrol = investigate = search = false;
    }

    internal void InvestigatePoint(Vector3 position)
    {
        pointOfInterest = position;
        investigate = true;
        stateMachine.SetCurrentState(investigateState);
    }
}
