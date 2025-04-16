using UnityEngine;


namespace Mouse {
        public class EnemyStats : CharacterStats
        {
            Animator animator;
            private Renderer enemyRenderer; // Reference to the Renderer component
            private Color originalColor; // Store the original color of the material
            public Color damageColor = Color.red; // Color to flash when taking damage
            public float flashDuration = 0.2f; // Duration of the red flash


            // Start is called once before the first execution of Update after the MonoBehaviour is created
            private void Awake()
            {
                healthLevel = 3;
                animator = GetComponent<Animator>();
                currentHealth = maxHealth;
            }
        
        private int SetMaxHealthFromHealthLevel()
        {
                maxHealth = healthLevel * 10;
                return maxHealth;
        }

            public void TakeDamage(int damage)
            {
               // Trigger the red flash effect
            if (enemyRenderer != null)
            {
                StartCoroutine(FlashRed());
            }

            // Check if health is 0 or below
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death");
                Destroy(gameObject, 2f); // Destroy the object after 2 seconds
            }
        }

        // Coroutine to flash red when taking damage
        private System.Collections.IEnumerator FlashRed()
        {
            // Change the material color to red
            enemyRenderer.material.color = damageColor;

            // Wait for the flash duration
            yield return new WaitForSeconds(flashDuration);

            // Revert the material color back to the original color
            enemyRenderer.material.color = originalColor;
        }
    }
}
