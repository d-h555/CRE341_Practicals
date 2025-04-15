using Unity.VisualScripting;
using UnityEngine;

namespace Mouse
{
    public class PursueTargetState : State
    {
        public override State RunCurrentState()
        {  
            //chase the target
            // if within attack range, check if player has drank potion. if so, switch to flee state
            // else switch to attack state
            return this;
        }
    }
    
}