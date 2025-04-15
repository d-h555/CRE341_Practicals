using Mouse;
using Unity.VisualScripting;
using UnityEngine;

namespace Mouse 
{
    public class MouseManager : MonoBehaviour
    {
        // Ensure the EnemyLocomotionManager class is defined or imported
        EnemyLocomotionManager enemyLocomotionManager;
        [Header("A.I Settings")]
        //the higher, and lower, respectively these angles are, the greator the detection filed of view
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public bool isPreformingAction;

        public float detectionRadius = 20f;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }
        private void Update ()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
        }
    }
}

