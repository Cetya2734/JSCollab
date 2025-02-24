using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCandles : MonoBehaviour, IInteractable
{
    public GameObject lighterOB;
    public GameObject flame;
    public GameObject lightText;

    private bool unlit;

    void Start()
    {
        unlit = true;
        flame.SetActive(false);
        lightText.SetActive(false);
    }

    public void Interact()
    { 
        if (lighterOB.activeInHierarchy && unlit)
        {
            flame.SetActive(true);
            lightText.SetActive(false);
            unlit = false;
        }
    }

    public GameObject InteractionText()
    {
        return lightText;
    }

    void OnTriggerEnter(Collider other)
    {

        if (unlit)
        {
            lightText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        lightText.SetActive(false);
    }
}
