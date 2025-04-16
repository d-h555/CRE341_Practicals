using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform

    void Update()
    {
        FaceTowardsPlayer();
    }

    private void FaceTowardsPlayer()
    {
        if (player == null) return; // Ensure the player reference is assigned

        // Calculate the direction to the player
        Vector3 direction = player.position - transform.position;

        // Keep the sprite facing only in the X-Z plane (ignore Y-axis)
        direction.y = 0;

        // Rotate the sprite to face the player
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
