using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBoxScript : MonoBehaviour, IInteractable
{
    public Animator boxOB;
    public GameObject keyOBNeeded;
    public GameObject openText;
    public GameObject keyMissingText;
    public AudioSource openSound;
    
    private WaitForSeconds interactionDelay = new WaitForSeconds(1f);

    private bool isOpen;

    void Start()
    {
        openText.SetActive(false);
        keyMissingText.SetActive(false);
    }

    public void Interact()
    {
        if (keyOBNeeded.activeInHierarchy == true)
        {
            keyOBNeeded.SetActive(false);
            openSound.Play();
            boxOB.SetBool("open", true);
            openText.SetActive(false);
            keyMissingText.SetActive(false);
            isOpen = true;
        }
        else if (keyOBNeeded.activeInHierarchy == false)
        {
            openText.SetActive(false);
            keyMissingText.SetActive(true);
            StartCoroutine(DisableTextAfterDelay());
        }

        if (isOpen)
        { 
            boxOB.GetComponent<BoxCollider>().enabled = false;
            boxOB.GetComponent<OpenBoxScript>().enabled = false;
        }
    }

    public GameObject InteractionText()
    {
        return openText;
    }

    void OnTriggerEnter(Collider other)
    {
        openText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        openText.SetActive(false);
        keyMissingText.SetActive(false);
    }
    
    private IEnumerator DisableTextAfterDelay()
    {
        // Wait for the specified duration
        yield return interactionDelay;

        // Disable the textOB
        keyMissingText.SetActive(false);    }
}
