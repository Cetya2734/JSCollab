using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ammo : MonoBehaviour, IInteractable
{
    public GameObject theAmmo;
    public GameObject weaponOB;
    public GameObject pickUpText;

    public AudioSource pickUpSound;

    public int ammoBoxAmount;

    public void Interact()
    {
        //weaponOB.GetComponent<Revolver>().maxAmmo += ammoBoxAmount;
        pickUpText.SetActive(false);
        theAmmo.SetActive(false);
        pickUpSound.Play();
        //weaponOB.GetComponent<Revolver>().UpdateAmmoText();
    }

    public GameObject InteractionText()
    {
       return pickUpText;
    }

    private void onDisable()
    {
        pickUpText.SetActive(false);
    }
}
