using Mouse;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Mouse 
{
    public class MouseManager : MonoBehaviour
    {
        // Ensure the EnemyLocomotionManager class is defined or imported
        EnemyLocomotionManager enemyLocomotionManager;
        AnimatorManager enemyAnimatorHandler;
        public MouseAttackAction currentAttack;
        public MouseAttackAction[] attackActions;
        Potion potion;
        [Header("A.I Settings")]
        //the higher, and lower, respectively these angles are, the greator the detection filed of view
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
      
        public bool isPreformingAction;
        public MouseAttackAction[] enemyAttacks;

        public float detectionRadius = 20f;
        public float currentRecoveryTime = 0;


        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorHandler = GetComponent<MouseAnimatorHandler>();
        }
        private void FixedUpdate ()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
        if (enemyLocomotionManager.currentTarget != null)
            {
                enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
            }

            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
            else if (potion != null && potion.HasBeenConsumed() == true) // Check if the potion has been consumed
            {
                Debug.Log("Player has consumed the potion!");
            enemyLocomotionManager.HandleFleeTarget();
            }
            else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
            {
                AttackTarget();
            }
        }

    private void HandleInRecovery()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPreformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPreformingAction = false;
                currentRecoveryTime = 0;
            }
        }
    }

        private void AttackTarget()
        {
            if(isPreformingAction)
                return;
            if (currentAttack != null && enemyLocomotionManager.currentTarget != null)
            {
                currentAttack = null;
                GetNewAttack();
            }
            else
            {
                isPreformingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }

        #region Attacks
        private void GetNewAttack()
        {
            Vector3 targetsDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;
            
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                MouseAttackAction mouseAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= mouseAttackAction.maxDistanceNeededToAttack && enemyLocomotionManager.distanceFromTarget >= mouseAttackAction.minDistanceNeededToAttack)
                {
                   if (viewableAngle <= mouseAttackAction.maxAttackAngle && viewableAngle >= mouseAttackAction.minAttackAngle)
                   {
                    maxScore += mouseAttackAction.attackScore;
                   }
                }
            }
        
        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
                    MouseAttackAction mouseAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= mouseAttackAction.maxDistanceNeededToAttack && enemyLocomotionManager.distanceFromTarget >= mouseAttackAction.minDistanceNeededToAttack)
                {
                   if (viewableAngle <= mouseAttackAction.maxAttackAngle && viewableAngle >= mouseAttackAction.minAttackAngle)
                   {
                    if (currentAttack != null)
                        return;

                    temporaryScore += mouseAttackAction.attackScore;

                        if(temporaryScore > randomValue)
                        {
                         currentAttack = mouseAttackAction;
                        }
                   }
                }
            }
        }
            #endregion

    }                        

}

