using UnityEngine;

namespace Mouse
{
    public class MouseAnimatorHandler : AnimatorManager
    {
        private MouseManager mouseManager;

        public Rigidbody rb;
            void Awake()
            {
                anim = GetComponent<Animator>();
                mouseManager = GetComponent<MouseManager>();
                rb = GetComponent<Rigidbody>();
            }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            mouseManager.rb.linearDamping = anim.rootPosition.magnitude;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = anim.deltaPosition / delta;
            mouseManager.rb.linearVelocity = velocity;
        }
    }    
}
