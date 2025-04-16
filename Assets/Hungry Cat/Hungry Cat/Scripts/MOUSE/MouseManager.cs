using System.Diagnostics;
using Mouse;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Mouse 
{
    public class MouseManager : MonoBehaviour
    {
        public State currentState;
      
        // Ensure the EnemyLocomotionManager class is defined or imported
        EnemyLocomotionManager enemyLocomotionManager;
        AnimatorManager enemyAnimatorHandler;
        public NavMeshAgent navMeshAgent;
        public Rigidbody rb;
        EnemyStats enemyStats;
    
        Potion potion;
        [Header("A.I Settings")]
        //the higher, and lower, respectively these angles are, the greator the detection filed of view
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

          [Header("Detection Settings")]
        public float detectionRadius = 20f; // Radius for detecting targets
        
        [Header("Attack Settings")]
        public float maxAttackRange = 1.5f; // Maximum range for attacks
        public float currentRecoveryTime = 0f; // Time remaining before the next attack
        public float recoveryTime = 2f; // Total recovery time between attacks
        public bool isPerformingAction; // Whether the mouse is currently performing an action
        public float distanceFromTarget;
      
        public bool isPreformingAction;
        public CharacterStats currentTarget; // Initialize this later in the code
        
        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorHandler = GetComponent<MouseAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            potion = GetComponent<Potion>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            rb.isKinematic = true;
        }
        private void FixedUpdate ()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);
                
                if (nextState != null)
                {
                    SwitchToNewState(nextState);
                }
            }
        }

        private void SwitchToNewState(State state)
        {
            if (currentState != null)
            {
                currentState.enabled = false;
            }
            currentState = state;
            currentState.enabled = true;
        }
    }

}