using UnityEngine;

public class AttackState : State

{

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
        Debug.Log("Attacking the player...");
        return this;
    }
}
