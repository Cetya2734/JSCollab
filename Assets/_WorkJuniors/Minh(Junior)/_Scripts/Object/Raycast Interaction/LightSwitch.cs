using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightSwitch : MonoBehaviour, IInteractable
{
    public GameObject onOB;
    public GameObject offOB;
    public GameObject lightsText;
    public GameObject lightOB;
    public AudioSource switchClick;

    private bool lightsAreOn;
    private bool lightsAreOff;

    void Start()
    {
        lightsAreOn = false;
        lightsAreOff = true;
        onOB.SetActive(false);
        offOB.SetActive(true);
        lightOB.SetActive(false);
    }

    public void Interact()
    {
        ToggleLights();
    }

    public GameObject GetGameObject()
    {
        return lightsText;
    }

    void OnTriggerEnter(Collider other)
    {
        lightsText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        lightsText.SetActive(false);
    }

    void ToggleLights()
    {
        if (lightsAreOn)
        {
            lightOB.SetActive(false);
            onOB.SetActive(false);
            offOB.SetActive(true);
            switchClick.Play();
            lightsAreOff = true;
            lightsAreOn = false;
        }
        else if (lightsAreOff)
        {
            lightOB.SetActive(true);
            onOB.SetActive(true);
            offOB.SetActive(false);
            switchClick.Play();
            lightsAreOff = false;
            lightsAreOn = true;
        }
    }
}
