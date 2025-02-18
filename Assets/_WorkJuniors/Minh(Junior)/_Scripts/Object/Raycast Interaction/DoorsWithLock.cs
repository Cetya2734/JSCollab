using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsWithLock : MonoBehaviour, IInteractable
{
    public Animator door;
    public GameObject openText;
    public GameObject KeyINV;

    public AudioSource doorSound;
    public AudioSource lockedSound;

    private bool isOpen;

    void Start()
    {
        //openText.SetActive(false);
        isOpen = false;
    }

    public void Interact()
    {
        if (!isOpen)
        {
            if (KeyINV.activeInHierarchy)
            {
                isOpen = true;
                door.SetBool("open", true);
                door.SetBool("closed", false);
                openText.SetActive(false);
                doorSound.Play();
            }
            else
            {
                lockedSound.Play();
            }
        }
        else
        {
            isOpen = false;
            door.SetBool("open", false);
            door.SetBool("closed", true);
            doorSound.Play();
        }
    }

    public GameObject InteractionText()
    {
        return openText;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        openText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        openText.SetActive(false);
    }
}
