using Mouse;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10; // Damage dealt to the enemy
    public float attackRange = 2f; // Range within which the player can attack
    public LayerMask enemyLayer; // Layer mask to identify enemies
    public Transform attackPoint; // Point from which the attack originates
    public Potion potion; // Reference to the Potion script

    // Update is called once per frame
    void Update()
    {
        // Check for attack input (e.g., left mouse button or "Fire1" input)
        // Check if the potion is consumed and for attack input
        if (potion != null && potion.isDrank && Input.GetButtonDown("Fire1"))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // Detect enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        // Loop through all detected enemies and apply damage
        foreach (Collider enemy in hitEnemies)
        {
            EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(attackDamage); // Apply damage to the enemy
            }
        }

        Debug.Log("Attack performed!"); // Debug message to indicate an attack was performed
    }

    // Visualize the attack range in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}