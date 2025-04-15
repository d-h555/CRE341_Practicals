using Mouse;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FleeState : State
{
    public float maxSpeed; // Maximum speed at which the mouse can flee
    public float maxSight; // Maximum distance at which the mouse can detect the player
    public GameObject player; // Reference to the player GameObject
    NavMeshAgent agent; // Reference to the NavMeshAgent for movement

    public Animator animator; // Reference to the Animator for controlling animations

    public float wanderSpeed; // Speed at which the mouse wanders when not fleeing
    public float wanderTime; // Time interval for changing the wandering direction
    private float timeToChangeDirection = 0; // Timer for changing the wandering direction
    public AnimationCurve fleeDistanceCurve; // Curve to determine flee distance based on proximity to the player

    private Vector3 randomDirection; // Random direction for wandering

    private GameObject playerPosition; // Cached reference to the player's position

    private void Start()
    {
        animator = GetComponent<Animator>(); // Initialize the Animator component
    }

    private void OnEnable()
    {
        animator.SetBool("isRunning", true); // Set the "isRunning" animation flag to true when the state is enabled
    }

    // This method is called when the state is running
    public override State RunCurrentState()
    {
        // If the player's position is not cached, find the player GameObject by tag
        if (playerPosition == null)
        {
            playerPosition = GameObject.FindWithTag("Player");
        }

        // If the player is found, calculate the flee behavior
        if (playerPosition != null)
        {
            // Calculate the direction and distance from the player
            Vector3 direction = transform.position - playerPosition.transform.position;
            Vector3 distance = transform.position - playerPosition.transform.position;

            // If the player is within the mouse's sight range
            if (distance.magnitude < maxSight)
            {
                Debug.Log("I can see the player"); // Debug message indicating the player is detected

                // Scale the distance based on the maximum sight range
                float distScaled = distance.magnitude / maxSight;

                // Use the flee distance curve to determine how far to flee
                float fleeDistance = fleeDistanceCurve.Evaluate(distScaled);

                // Move the mouse away from the player based on the flee distance and speed
                transform.position += direction.normalized * fleeDistance * fleeDistance * maxSpeed * Time.deltaTime;
            }
            else
            {
                // If the player is not within sight, wander in a random direction
                if (timeToChangeDirection <= 0)
                {
                    // Generate a random direction for wandering
                    float z = UnityEngine.Random.Range(-1.0f, 1.0f);
                    float x = UnityEngine.Random.Range(-1.0f, 1.0f);
                    randomDirection = new Vector3(x, 0, z);

                    // Reset the timer for changing direction
                    timeToChangeDirection = wanderTime;
                }
            }
        }

        // Return the current state as a fallback
        return this;
    }

    // This method is called every frame to handle the logic for this state
    public override State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager animatorHandler)
    {
        throw new System.NotImplementedException(); // Placeholder for functionality if needed
    }
}