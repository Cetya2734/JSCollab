using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyPad : MonoBehaviour, IInteractable
{
    public GameObject keypadOB;
    public GameObject keypadText;

    void Start()
    {
    }

    public void Interact()
    {
        keypadOB.SetActive(true);
    }
    public GameObject InteractionText()
    {
        return keypadText;
    }

    void OnTriggerEnter(Collider other)
    {
        keypadText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        keypadText.SetActive(false);
    }
}
