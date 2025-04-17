using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace Mouse
{
    public class StateManager : MonoBehaviour
    {
        State currentState;
        void Update()
        {
            RunStateMachine();
        }

        private void RunStateMachine()
        {
            State nextState = currentState?.RunCurrentState();

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
}