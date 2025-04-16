using UnityEngine;

namespace Mouse
{

        // This class is responsible for managing the player's stats, such as health, stamina, and other attributes.
    public class PlayerStats : CharacterStats // Ensure CharacterStats is defined or imported
    {
        // Currently, it serves as a placeholder for future development.
    
        AnimatorManager animatorManager;

        private void Awake()
        {
            animatorManager = GetComponent<AnimatorManager>();
        }

        void Start ()
        {
            healthLevel = 5; // Set the health level to 3
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public Transform currentTarget;

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

                  // Play the hurt sound
            //if (audioSource != null && hurtSound != null)
            //{
                //audioSource.PlayOneShot(hurtSound);
            //}

            // Optional: Add logic for when the player dies
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log("Player is dead!");
                // Add death logic here
            }
        }
    }
}