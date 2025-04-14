using UnityEngine;

public class IdleState : State
{
    Potion isDrank = new Potion();
    public ChaseState chaseState;
    public FleeState fleeState;
    public bool canSeeThePlayer;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator.SetBool("isRunning", false);

    }
    public override State RunCurrentState()
    {
      if (canSeeThePlayer && isDrank == true)
      {
        return fleeState;
      }
     
     
        if (canSeeThePlayer && isDrank == false)
        {
            return chaseState;
        }
        else
        {
            // Logic for idling can be added here
            Debug.Log("Idling...");
            return this;
        }
    }
}
