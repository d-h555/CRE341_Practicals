using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfStars {get ; private set; }

    public UnityEvent<int> OnStarsCollected;
    
    private void Start()
    {
        if (OnStarsCollected == null)
        {
            OnStarsCollected = new UnityEvent<int>();
        }
    }

    public void StarsCollected()
    {
        NumberOfStars++;
        OnStarsCollected?.Invoke(NumberOfStars);
    }
}
