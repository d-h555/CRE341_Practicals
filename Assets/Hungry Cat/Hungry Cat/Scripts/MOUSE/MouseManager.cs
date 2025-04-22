using UnityEngine;
using UnityEngine.AI;
using System;

namespace Mouse
{
    public class MouseManager : MonoBehaviour
    {
        public State currentState;

        // References to other components
        EnemyLocomotionManager enemyLocomotionManager;
        AnimatorManager enemyAnimatorHandler;
        public NavMeshAgent navMeshAgent;
        public Rigidbody rb;
        EnemyStats enemyStats;

        Potion potion;

        [Header("A.I Settings")]
        public float maximumDetectionAngle = 50; // Maximum detection angle
        public float minimumDetectionAngle = -50; // Minimum detection angle

        [Header("Detection Settings")]
        public float detectionRadius = 20f; // Radius for detecting targets

        [Header("Attack Settings")]
        public float maxAttackRange = 1.5f; // Maximum range for attacks
        public float currentRecoveryTime = 0f; // Time remaining before the next attack
        public float recoveryTime = 2f; // Total recovery time between attacks
        public bool isPerformingAction; // Whether the mouse is currently performing an action
        public float distanceFromTarget;
        private PlayerStats playerStats; // Reference to the PlayerStats script
        public PlayerStats currentTarget; // Reference to the current target

        MouseManager mouseManager; // Reference to the MouseManager script

        private void Awake()
        {
            // Initialize references to required components
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorHandler = GetComponent<MouseAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            potion = GetComponent<Potion>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            mouseManager.enabled = true; // Enable the MouseManager script
        }

        private void FixedUpdate()
        {
            UpdatePlayerStats(); // Update the reference to PlayerStats
            rb.isKinematic = true; // Ensure the Rigidbody is kinematic
            HandleStateMachine(); // Handle the state machine logic
            DetectTarget(); // Detect the player or other targets
        }

        private void UpdatePlayerStats()
        {
            // Update the PlayerStats reference if a target is assigned
            if (currentTarget != null)
            {
                playerStats = currentTarget.GetComponent<PlayerStats>();
                if (playerStats == null)
                {
                    Debug.LogWarning("PlayerStats component not found on the current target!");
                }
            }
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                // Tick the current state and determine the next state
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

                if (nextState != null && nextState != currentState)
                {
                    SwitchToNewState(nextState);
                }
                else if (nextState == null)
                {
                    Debug.LogWarning("State machine returned null. Staying in the current state.");
                }
            }
            else
            {
                Debug.LogError("No current state assigned to the state machine!");
            }
        }

        private void SwitchToNewState(State state)
        {
            // Disable the current state and switch to the new state
            if (currentState != null)
            {
                currentState.enabled = false;
            }

            currentState = state;
            currentState.enabled = true;
            Debug.Log("Switched to new state: " + state.GetType().Name);
        }

       private void DetectTarget()
        {
            // Detect objects within the detection radius on the "Player" layer
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Player"));
            for (int i = 0; i < colliders.Length; i++)
            {
                // Try to get the PlayerStats component from the detected object
                PlayerStats detectedPlayerStats = colliders[i].transform.GetComponent<PlayerStats>();
                if (detectedPlayerStats != null)
                {
                    currentTarget = detectedPlayerStats; // Set the detected PlayerStats as the current target
                    Debug.Log("Target detected: " + currentTarget.name);
                    break;
                }
            }

            // If no target is found, clear the current target
            if (currentTarget == null)
            {
                Debug.Log("No target detected.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the mouse enters a trigger collider with the "Player" tag
            if (other.CompareTag("Player"))
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    currentTarget = playerStats; // Set the detected PlayerStats as the current target
                    Debug.Log("Target detected: " + currentTarget.name);
                }
            }
        }
    }
}