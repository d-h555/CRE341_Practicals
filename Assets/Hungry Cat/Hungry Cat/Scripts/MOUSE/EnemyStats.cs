using UnityEngine;


namespace Mouse {
        public class EnemyStats : CharacterStats
        {
            public int healthLevel = 10;
            public int maxHealth;
            public int currentHealth;
            Animator animator;

            // Start is called once before the first execution of Update after the MonoBehaviour is created
            private void Awake()
            {
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
            currentHealth = currentHealth - damage;

            animator.Play("Damage");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death");
                Destroy(gameObject, 2f); // Destroy the object after 2 seconds
            }
        }
    }
}