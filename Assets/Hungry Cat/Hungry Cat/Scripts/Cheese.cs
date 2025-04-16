using UnityEngine;

public class Star : MonoBehaviour
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
    }
  }
     private void Update()
    {
        // Apply a sine wave to make the cheese hover
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

}
