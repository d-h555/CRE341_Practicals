using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCheese {get ; private set; }

    public UnityEvent<int> OnCheeseCollected;
    
    private void Start()
    {
        if (OnCheeseCollected == null)
        {
            OnCheeseCollected = new UnityEvent<int>();
        }
    }

    public void CheeseCollected()
    {
        NumberOfCheese++;
        OnCheeseCollected?.Invoke(NumberOfCheese);
    }
}
