using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneBehaviourController : MonoBehaviour
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

    DroneSuspicionManager droneSuspicionManager;

    int searchRadius = 5;

    #region Initialize states and navmeshes
    private void InitializeStates()
    {
        patrolState = new PatrolState("Patrolling");
        investigateState = new InvestigateState("Investigating");
        searchingState = new SearchingState("Searching");
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
        droneSuspicionManager = FindObjectOfType<DroneSuspicionManager>();
        InitializeStates();
        InitializeNavMeshAgent();
        ResetState();
    }

    internal void ResetState()
    {
        patrolState.SetPatrolPath(patrolPath);
        stateMachine.SetCurrentState(patrolState);
        previousState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.menuOpened)
        {
            stateMachine.GetCurrentState().GetNavMeshAgent().isStopped = false;
            if (!HasStateChanged(stateMachine.GetCurrentState())) { ChangeState(); }
            if (stateMachine.GetCurrentState().GetIsFinished()) { patrol = true; }
            stateMachine.GetCurrentState().PerformAction();
        }
        else
        {
            stateMachine.GetCurrentState().GetNavMeshAgent().isStopped = true;
        }
    }

    private bool HasStateChanged(State currentState)
    {
        if (!currentState == previousState)
        {
            stateChanged = true;
        }
        return stateChanged;
    }

    private void ChangeState()
    {
        previousState = stateMachine.GetCurrentState();
        if (search)
        {
            searchingState.InitializeSearchParameters(pointOfInterest, searchRadius);
            stateMachine.SetCurrentState(searchingState);
        }
        else if (investigate)
        {
            investigateState.InitializeSearchParameters(pointOfInterest, searchRadius);
            stateMachine.SetCurrentState(investigateState);
        }
        else if (patrol)
        {
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
        SetupPointOfIntrestAndTransitionToDiffState(position);
        droneSuspicionManager.heardSound(position);
        ChangeState();
    }

    private void SetupPointOfIntrestAndTransitionToDiffState(Vector3 position)
    {
        pointOfInterest = position;
        if (GetStateName() == "Searching")
        {
            stateMachine.SetCurrentState(searchingState);
            search = true;
        }
        else
        {
            stateMachine.SetCurrentState(investigateState);
            investigate = true;
        }
    }

    internal bool InAlertRange(Vector3 lastPosition, float soundReactionDistance)
    {
        return Vector3.Distance(transform.position, lastPosition) <= soundReactionDistance;
    }

    internal void ReactToSound(Vector3 position)
    {
        SetupPointOfIntrestAndTransitionToDiffState(position);
    }

    internal void ChangeToSearchingState(Vector3 location)
    {
        pointOfInterest = (location + transform.position) / 2;
        search = true;
        stateMachine.SetCurrentState(searchingState);
        ChangeState();
    }

    public string GetStateName()
    {
        if (stateMachine.GetCurrentState() == searchingState)
        {
            return "Searching";
        }
        else if (stateMachine.GetCurrentState() == investigateState)
        {
            return "Investigating";
        }
        else
        {
            return "Patrolling";
        }
    }
}
