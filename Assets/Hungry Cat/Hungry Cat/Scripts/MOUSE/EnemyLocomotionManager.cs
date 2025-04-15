using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Mouse
{
public class EnemyLocomotionManager : MonoBehaviour
    {
        MouseManager mouseManager;
        
        MouseAnimatorHandler mouseAnimatorHandler;
        public Rigidbody rb;
        CharacterStats characterStats;
        public LayerMask detectionLayer;
        public static bool isPreformingAction;

        private void Awake()
        {
            mouseManager = GetComponent<MouseManager>();
            mouseAnimatorHandler = GetComponent<MouseAnimatorHandler>();
        }

        private void Start()
        {
            mouseManager.enabled = false;
            rb.isKinematic = false;
        }
    }
}
