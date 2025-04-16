using UnityEngine;

public class Potion : MonoBehaviour
{
    public bool isDrank; // Flag to check if the potion is consumed
    public Transform player; // Reference to the player object
    public float effectDuration = 15f; // Duration of the potion effect
    private float startTime; // Time when the potion effect started

        private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player picked up the potion!");
            player = other.transform; // Assign the player reference
            HasBeenConsumed(); // Apply the potion effect
        }
    }
     public bool HasBeenConsumed()
    {
        if (!isDrank)
        {
            // Add potion effect here
            startTime = Time.time; // Record the time when the potion is consumed
            isDrank = true; // Set the flag to true when the potion is consumed
            player.transform.localScale += new Vector3(0.6f, 0.6f, 0.6f); // Increase player size
            
            Debug.Log("Potion effect applied!");
            Destroy(gameObject); // Destroy the potion after use
            
            return true;
        }
        return false; // Return false if the potion has already been consumed
    }
        
        private void Update()
        {
            // Check if the potion effect duration has passed
            if (isDrank && Time.time - startTime > effectDuration)
            {
                // Remove potion effect here
                player.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f); // reset player size
                Debug.Log("Potion effect ended!");
                isDrank = false; // Reset the flag
            }
        }
    }


