using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySwitch : MonoBehaviour
{

    public int selectedWeapon = 0;
    public bool isSwitchingDisabled = false; // Flag to control weapon switching

    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
    }
    void Update()
    {
        if (!isSwitchingDisabled)
        {
            int previousSelectedWeapon = selectedWeapon;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (Input.GetButtonDown("1"))
            {
                selectedWeapon = 0;
            }

            if (Input.GetButtonDown("2") && transform.childCount >= 2)
            {
                selectedWeapon = 1;
            }

            if (Input.GetButtonDown("3") && transform.childCount >= 3)
            {
                selectedWeapon = 2;
            }

            if (Input.GetButtonDown("4") && transform.childCount >= 4)
            {
                selectedWeapon = 3;
            }

            if (Input.GetButtonDown("5") && transform.childCount >= 4)
            {
                selectedWeapon = 4;
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                selectWeapon();
            }
        }
    }

    void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
