using Mouse;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    Potion potion;
    public ChaseState chaseState;
    public FleeState fleeState;
    public bool canSeePlayer;
    public LayerMask detectionLayer;

    public GameObject waypointPrefab; // Prefab for waypoints
    public Transform[] waypoints; // Array of waypoints for wandering
    public float waypointReachedThreshold = 1f; // Distance to consider the waypoint reached
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private PlayerStats playerStats;

    private void Start()
    {
        // Find all waypoints in the scene by tag
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");

        // Convert the GameObjects to Transforms
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        Debug.Log("Waypoints found: " + waypoints.Length); // Debug log to check the number of waypoints found

        playerStats = Object.FindFirstObjectByType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found in the scene!");
        }
    }

    public override State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager animatorHandler)
    {
        if (canSeePlayer == false)
        {
            #region Handle enemy's detection of player
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
                    canSeePlayer = true; // Update the variable
                    PlayerStats playerStats = Object.FindFirstObjectByType<PlayerStats>();
                    if (playerStats != null)
                    {
                        mouseManager.currentTarget = playerStats.currentTarget.gameObject.GetComponent<PlayerStats>();
                    }

                    // Check the potion's state and transition accordingly
                    if (potion != null && potion.isDrank)
                    {
                        Debug.Log("Potion is consumed. Switching to flee state.");
                        return fleeState; // Switch to flee state if the potion is consumed
                    }
                    else
                    {
                        Debug.Log("Potion is not consumed. Switching to chase state.");
                        return chaseState; // Switch to chase state if the potion is not consumed
                    }
                }
            }
            else
            {
                canSeePlayer = false; // Reset if no player is detected
            }
            #endregion
        }

        // Handle waypoint wandering if no player is detected
        Wander(mouseManager);

        #region Handle switching to different states
        if (mouseManager.currentTarget)
        {
            // Check the potion's state and transition accordingly
            if (potion != null && potion.isDrank)
            {
                Debug.Log("Potion is consumed. Switching to flee state.");
                return fleeState; // Switch to flee state if the potion is consumed
            }

            Debug.Log("Potion is not consumed. Switching to chase state.");
            return chaseState; // Switch to chase state if the potion is not consumed
        }

        return this; // Stay in the idle state if no target is detected
        #endregion
    }

    private void Wander(MouseManager mouseManager)
    {
        // Check if waypoints are assigned
        if (waypoints == null || waypoints.Length == 0) return;

        // Ensure the NavMeshAgent is assigned
        if (mouseManager.navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned to the MouseManager!");
            return;
        }

        // Get the current waypoint
        Transform currentWaypoint = waypoints[currentWaypointIndex];

        // Move towards the current waypoint
        mouseManager.navMeshAgent.SetDestination(currentWaypoint.position);

        // Check if the mouse has reached the waypoint
        if (Vector3.Distance(mouseManager.transform.position, currentWaypoint.position) <= waypointReachedThreshold)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop back to the first waypoint
        }
    }

    public override State RunCurrentState()
    {
        throw new System.NotImplementedException();
    }
}