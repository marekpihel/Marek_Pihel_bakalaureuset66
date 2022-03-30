using UnityEngine;

public class StateMachine : MonoBehaviour
{
    State currentState;

    public State GetCurrentState() { return currentState; }
    public void SetCurrentState(State newState) { currentState = newState; }
}