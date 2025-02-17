using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;

    [SerializeField] private  AudioSource turnOn;
    [SerializeField] private  AudioSource turnOff;

    private bool on;
    private bool off;


    void Start()
    {
        off = true;
        flashlight.SetActive(false);
    }


    void Update()
    {
        if(off && Input.GetButtonDown("F"))
        {
            flashlight.SetActive(true);
            turnOn.Play();
            off = false;
            on = true;
        }
        else if (on && Input.GetButtonDown("F"))
        {
            flashlight.SetActive(false);
            turnOff.Play();
            off = true;
            on = false;
        }
    }
}
