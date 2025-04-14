using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public bool isPlayerInRange;

    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator.SetBool("isRunning", true);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override State RunCurrentState()
    {
       if (isPlayerInRange)
        {
            return attackState;
        }
        else
        {
            // Logic for chasing the player can be added here
            Debug.Log("Chasing the player...");
            return this;
        }
    }
}
