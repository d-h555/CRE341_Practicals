using UnityEngine;

public class IdleState : State
{
    Potion isDrank;
    public ChaseState chaseState;
    public FleeState fleeState;
    public bool canSeePlayer;
    public override State RunCurrentState()
    {
       if(canSeePlayer && isDrank != true)
       {
        return fleeState;
       }
        else if (canSeePlayer && isDrank == false)
        {
        return chaseState;
        }
        else
        {
        return this;
        }
    
    }
}
