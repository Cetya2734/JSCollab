using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKeyWithRaycast : MonoBehaviour, IInteractable
{
    public GameObject keyOB;
    public GameObject invOB;
    public GameObject pickUpText;
    public AudioSource keySound;

    void Start()
    {
        pickUpText.SetActive(false);
        invOB.SetActive(false);
    }

    public GameObject InteractionText()
    {
        return pickUpText;
    }

    public void Interact()
    {
        keyOB.SetActive(false);
        keySound.Play();
        invOB.SetActive(true);
        pickUpText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        pickUpText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        pickUpText.SetActive(false);
    }
}
