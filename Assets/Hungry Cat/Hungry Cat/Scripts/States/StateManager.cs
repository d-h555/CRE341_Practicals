using UnityEngine;

public class StateManager : MonoBehaviour
{
     State currentState;
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
{
    State nextState = currentState.RunCurrentState();
    
    if (nextState != null)
    {
        SwitchToTheNextState(nextState);
    }
}

private void SwitchToTheNextState(State nextState)
{
    currentState = nextState;
}


}
// This script manages the state machine for the game object it is attached to. It runs the current state and switches to the next state if needed.