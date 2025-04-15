using Mouse;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState; // Reference to the attack state

    // This method is called when the state is running
    public override State RunCurrentState()
    {
        throw new System.NotImplementedException(); // Placeholder for functionality if needed
    }

    // This method is called every frame to handle the logic for this state
    public override State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager mouseAnimatorHandler)
    {
        // Ensure there is a target to chase
        if (mouseManager.currentTarget == null)
        {
            return this; // Stay in the current state if no target is available
        }

        // Calculate the direction to the target
        Vector3 targetDirection = mouseManager.currentTarget.transform.position - transform.position;

        // Calculate the distance from the target
        float distanceFromTarget = Vector3.Distance(mouseManager.currentTarget.transform.position, transform.position);

        // Calculate the angle between the target's direction and the forward direction
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        // If the target is out of attack range, enable the running animation
        if (distanceFromTarget > mouseManager.maxAttackRange)
        {
            mouseAnimatorHandler.anim.SetBool("isRunning", true); // Set the "isRunning" animation flag to true
        }
        // If the target is within attack range, disable the running animation
        else if (distanceFromTarget <= mouseManager.maxAttackRange)
        {
            mouseAnimatorHandler.anim.SetBool("isRunning", false); // Set the "isRunning" animation flag to false
        }

        // If the target is within attack range, switch to the attack state
        if (mouseManager.distanceFromTarget <= mouseManager.maxAttackRange)
        {
            return attackState; // Transition to the attack state
        }
        // If the target is out of attack range, stay in the chase state
        else if (mouseManager.distanceFromTarget > mouseManager.maxAttackRange)
        {
            return this; // Stay in the chase state
        }

        // If no conditions are met, stay in the current state as a fallback
        return this;
    }
}