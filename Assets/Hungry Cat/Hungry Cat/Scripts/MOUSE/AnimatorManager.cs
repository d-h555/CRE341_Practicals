using UnityEngine;

namespace Mouse
{
public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;

// Ensure this is the correct file for AnimatorManager class

    // Existing methods and properties

        public void PlayTargetAnimation(string animationName, bool isInteracting, bool isDead = false)
        {
            // Add logic to play the animation using Unity's Animator component
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsInteracting", isInteracting);
                animator.SetBool("IsDead", isDead);
                animator.CrossFade(animationName, 0.2f);
            }
        }

    }
}