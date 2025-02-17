using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, IInteractable
{
    public Animator door;
    public GameObject openText;
    public AudioSource doorSound;

    void Start()
    {
        openText.SetActive(false);
    }

    public void Interact()
    {
        // Check if the doors are open or closed, and toggle their state accordingly
        if (door.GetBool("open"))
        {
            DoorOpens();
        }
        else
        {
            DoorCloses();
        }
    }

    public GameObject GetGameObject()
    {
        return openText;
    }
    
    void DoorOpens()
    {
        door.SetBool("open", false);
        door.SetBool("closed", true);
        doorSound.Play();
    }

    void DoorCloses()
    {
        door.SetBool("open", true);
        doorSound.Play();
    }
}
