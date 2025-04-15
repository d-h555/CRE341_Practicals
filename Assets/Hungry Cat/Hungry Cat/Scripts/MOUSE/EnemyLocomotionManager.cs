using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace Mouse
{
public class EnemyLocomotionManager : MonoBehaviour
    {
        MouseManager mouseManager;
        NavMeshAgent navMeshAgent;
        MouseAnimatorHandler mouseAnimatorHandler;
        public Rigidbody rb;
        public CharacterStats currentTarget;
        CharacterStats characterStats;
        public LayerMask detectionLayer;

        public float stoppingDistance;
        public float distanceFromTarget;
        private void Awake()
        {
            mouseManager = GetComponent<MouseManager>();
            mouseAnimatorHandler = GetComponent<MouseAnimatorHandler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }
        public void HandleDetection()
        {
           Collider[] colliders = Physics.OverlapSphere(transform.position, mouseManager.detectionRadius, detectionLayer);
        
            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
            }

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
               
                if (viewableAngle > mouseManager.minimumDetectionAngle && viewableAngle < mouseManager.maximumDetectionAngle)
                {
                   currentTarget = characterStats;
                }
            }
        }
    


        public void HandleMoveToTarget()

        {
            if (EnemyLocomotionManager.isPreformingAction)
            {
                return;
            }
            
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (mouseManager.isPreformingAction)
            //if we are preforming an action, stop our movement
            {
                mouseAnimatorHandler.anim.SetBool("isRunning", false);
                navMeshAgent.enabled = false;
            }   
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    mouseAnimatorHandler.anim.SetBool("isRunning", true);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    mouseAnimatorHandler.anim.SetBool("isRunning", false);
                }
            }
        }
    }
}
