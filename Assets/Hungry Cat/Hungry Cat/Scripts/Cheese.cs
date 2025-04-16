using UnityEngine;

public class Cheese : MonoBehaviour
{
  
    public float hoverHeight = 0.5f; // Height of the hover effect
    public float hoverSpeed = 2f; // Speed of the hover effect

    private Vector3 startPosition; // Initial position of the cheese

    private void Start()
    {
        // Store the initial position of the cheese
        startPosition = transform.position;
    }
  private void OnTriggerEnter(Collider other)
  {
    PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

    if(playerInventory != null)
    {
        playerInventory.CheeseCollected();
        gameObject.SetActive(false);

        // Check if the player has collided with the cheese
         if (other.CompareTag("Player"))
         {
             MazeGenerator mazeGenerator = FindFirstObjectByType<MazeGenerator>();
             if (mazeGenerator != null)
             {
                 mazeGenerator.CollectCheese(); // Notify the MazeGenerator
             }
 
             gameObject.SetActive(false); // Deactivate the cheese
         }
    }
  }
     private void Update()
    {
          // Apply a sine wave to make the cheese hover
    float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

    // Ensure the cheese doesn't go below the initial position (floor level)
    newY = Mathf.Max(newY, startPosition.y);

    // Update the position of the cheese
    transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
