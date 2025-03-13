using UnityEngine;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI starText;
    private PlayerInventory playerInventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        starText = GetComponent<TextMeshProUGUI>();
        playerInventory = FindAnyObjectByType<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.OnStarsCollected.AddListener(UpdateStarText);
        }
    }

    public void UpdateStarText(int newStarCount)
    {
        if (playerInventory != null)
        {
            starText.text = newStarCount.ToString();
        }
    }
}
