using Mouse;
using UnityEngine;

public class IdleState : State
{
    
    Potion isDrank;
    public ChaseState chaseState;
    public FleeState fleeState;
    public bool canSeePlayer;
    public LayerMask detectionLayer;
    public Transform[] waypoints; // Array of waypoints for wandering
    public float waypointReachedThreshold = 1f; // Distance to consider the waypoint reached
    private int currentWaypointIndex = 0; // Index of the current waypoint

    public override State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager animatorHandler)
    {
    if (canSeePlayer == false)
    {
        #region handle enemy's detection of player
        Collider[] colliders = Physics.OverlapSphere(transform.position, mouseManager.detectionRadius, detectionLayer);
    
        for (int i = 0; i < colliders.Length; i++)
        {
            enemyStats = colliders[i].transform.GetComponent<EnemyStats>();
        }

        if (enemyStats != null)
        {
            Vector3 targetDirection = enemyStats.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            
            if (viewableAngle > mouseManager.minimumDetectionAngle && viewableAngle < mouseManager.maximumDetectionAngle)
            {
                mouseManager.currentTarget = enemyStats;
                return chaseState; // Ensure a return value here
            }
        }
    }

    #endregion

        // Handle waypoint wandering
        Wander(mouseManager);

        #region Handle switching to different states
        if (mouseManager.currentTarget)
        {
            return fleeState; // Switch to flee state if the player is detected
        }

        return this; // Stay in the idle state
        #endregion
    }

    private void Wander(MouseManager mouseManager)
    {
        // Check if waypoints are assigned
        if (waypoints == null || waypoints.Length == 0) return;

        // Get the current waypoint
        Transform currentWaypoint = waypoints[currentWaypointIndex];

        // Move towards the current waypoint
        mouseManager.navMeshAgent.SetDestination(currentWaypoint.position);

        // Check if the mouse has reached the waypoint
        if (Vector3.Distance(mouseManager.transform.position, currentWaypoint.position) <= waypointReachedThreshold)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public override State RunCurrentState()
    {
        throw new System.NotImplementedException();
    }

}
