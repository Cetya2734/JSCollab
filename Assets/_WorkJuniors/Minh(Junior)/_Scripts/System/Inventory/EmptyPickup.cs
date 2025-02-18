using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPickup : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject emptyGameObject;

    public void Interact()
    {
        
    }
    public GameObject InteractionText()
    {
        return emptyGameObject;
    }
}
