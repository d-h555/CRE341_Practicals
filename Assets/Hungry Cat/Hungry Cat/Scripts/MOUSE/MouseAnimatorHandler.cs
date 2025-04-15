using UnityEngine;

namespace Mouse
{
    public class MouseAnimatorHandler : AnimatorManager
    {
        void Awake()
        {
            anim = GetComponent<Animator>();
        } 
    }
}