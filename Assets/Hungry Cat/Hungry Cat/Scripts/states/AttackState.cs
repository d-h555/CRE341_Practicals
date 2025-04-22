using UnityEngine;
namespace Mouse
{
public class AttackState : State
{
            public ChaseState chaseState; // Reference to the chase state
            public override State RunCurrentState()
            {
                throw new System.NotImplementedException();
            }

public override State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager animatorHandler)
{
    // Add logic to determine the next state or return null as a fallback
    PerformAttack(mouseManager, null); // Assuming null for MouseAnimatorHandler for now
    return chaseState; // Return the chaseState as the next state
}

        private void PerformAttack(MouseManager mouseManager, MouseAnimatorHandler mouseAnimatorHandler)
        {
            if (mouseManager.isPerformingAction)
            {
                Debug.Log("Already performing an action. Attack skipped.");
                return; // Prevent multiple attacks if already performing an action
            }

            // Set the recovery time for the next attack
            mouseManager.currentRecoveryTime = mouseManager.recoveryTime;

            // Set the performing action flag to true
            mouseManager.isPerformingAction = true;

            // Cause damage to the player
            if (mouseManager.currentTarget != null)
            {
                PlayerStats playerStats = mouseManager.currentTarget.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(10); // Deal 10 damage to the player
                    Debug.Log($"Dealt 10 damage to {playerStats.name}");
                }
                else
                {
                    Debug.LogWarning("PlayerStats component not found on the target.");
                }
            }

            // Reset the performing action flag after the attack animation or delay
            mouseManager.isPerformingAction = false;
            Debug.Log("Attack performed!");
        }
    }
}
