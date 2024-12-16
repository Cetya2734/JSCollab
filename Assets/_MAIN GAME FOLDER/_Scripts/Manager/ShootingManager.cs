using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    // Declare an event that other classes can subscribe to
    public static event Action OnShoot;

    // This method will be called when the player shoots
    public void TriggerShootEvent()
    {
        OnShoot?.Invoke();  // Trigger the shoot event
    }
}
