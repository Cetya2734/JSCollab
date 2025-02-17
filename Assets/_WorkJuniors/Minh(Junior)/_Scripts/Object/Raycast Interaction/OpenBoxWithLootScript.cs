using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBoxWithLootScript : MonoBehaviour, IInteractable
{
    public Animator boxOB;
    public GameObject keyOBNeeded;
    public GameObject openText;
    public GameObject keyMissingText;
    public AudioSource openSound;

    public GameObject drop1;
    public GameObject drop2;
    public GameObject drop3;
    public GameObject drop4;
    public GameObject drop5;
    public GameObject drop6;

    private bool isOpen;
    private int randomNumber;
    private bool boxOpened;

    void Start()
    {
        randomNumber = Random.Range(0, 5);
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

            if (randomNumber == 0)
            {
                drop1.SetActive(true);
            }
            else if (randomNumber == 1)
            {
                drop2.SetActive(true);
            }
            else if (randomNumber == 2)
            {
                drop3.SetActive(true);
            }
            else if (randomNumber == 3)
            {
                drop4.SetActive(true);
            }
            else if (randomNumber == 4)
            {
                drop5.SetActive(true);
            }
            else if (randomNumber == 5)
            {
                drop6.SetActive(true);
            }

            boxOpened = true;
        }
        else if (!boxOpened && keyOBNeeded.activeInHierarchy == false)
        {
            openText.SetActive(false);
            keyMissingText.SetActive(true);
        }

        if (isOpen)
        {
            boxOB.GetComponent<BoxCollider>().enabled = false;
            boxOB.GetComponent<OpenBoxScript>().enabled = false;
        }
    }

    public GameObject GetGameObject()
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
}
