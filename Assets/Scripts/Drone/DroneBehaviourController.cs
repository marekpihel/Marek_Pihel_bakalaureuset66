using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneBehaviourController : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    List<GameObject> patrolPath;

    bool search = false, investigate = false, patrol = false, stateChanged = false;
    Vector3 pointOfInterest;

    StateMachine stateMachine;
    PatrolState patrolState;
    InvestigateState investigateState;
    SearchingState searchingState;

    State previousState;

    DroneSuspicionManager droneSuspicionManager;

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
            if (!HasStateChanged(stateMachine.GetCurrentState())) 
            {
                ChangeState();
            }
            if (stateMachine.GetCurrentState().GetIsFinished())
            {
                patrol = true;
            }
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
            searchingState.InitializeSearchParameters(pointOfInterest, droneSuspicionManager.GetSearchReadius());
            stateMachine.SetCurrentState(searchingState);
        }
        else if (investigate)
        {
            investigateState.InitializeSearchParameters(pointOfInterest, droneSuspicionManager.GetSearchReadius());
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
        droneSuspicionManager.HeardSound(position, this);
        ChangeState();
    }

    private void SetupPointOfIntrestAndTransitionToDiffState(Vector3 position)
    {
            pointOfInterest = position;
            transform.LookAt(Vector3.Lerp(pointOfInterest, transform.position, 0.1f));
            stateMachine.SetCurrentState(investigateState);
            investigate = true;
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
