using Mouse;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState; // Reference to the state for pursuing the target
    public override State RunCurrentState()
    {
        throw new System.NotImplementedException();
    }

    // This method is called every frame to handle the logic for this state
    public override State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager mouseAnimatorHandler)
    {
        // Calculate the distance from the target
        mouseManager.distanceFromTarget = Vector3.Distance(mouseManager.currentTarget.transform.position, mouseManager.transform.position);

        // Check if the attack cooldown is over and the target is within attack range
        if (mouseManager.currentRecoveryTime <= 0 && mouseManager.distanceFromTarget <= mouseManager.maxAttackRange)
        {
            PerformAttack(mouseManager, mouseAnimatorHandler); // Perform the attack
            return this; // Stay in the current state
        }

        // If the target is out of range, switch to the pursue target state
        if (mouseManager.distanceFromTarget > mouseManager.maxAttackRange)
        {
            return chaseState;
        }

        // Return the current state as a fallback
        return this;
    }

    // Simple attack function
    private void PerformAttack(MouseManager mouseManager, AnimatorManager mouseAnimatorHandler)
    {
        if (mouseManager.isPreformingAction) return; // Prevent multiple attacks if already performing an action

        // Play the attack animation
        mouseAnimatorHandler.anim.SetTrigger("isAttacking"); // Trigger the attack animation

        // Set the recovery time for the next attack
        mouseManager.currentRecoveryTime = mouseManager.recoveryTime;

        // Set the performing action flag to true
        mouseManager.isPreformingAction = true;

          // Cause damage to the player
         if (mouseManager.currentTarget != null)
         {
            PlayerStats playerStats = mouseManager.currentTarget.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                  playerStats.TakeDamage(10); // Deal 10 damage to the player
            }
         }

        Debug.Log("Attack performed!"); // Debug message to indicate an attack was performed
    }
}