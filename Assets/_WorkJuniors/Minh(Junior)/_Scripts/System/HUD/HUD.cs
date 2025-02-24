using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject flashLightON;
    public GameObject flashLightOFF;
    public GameObject flashLightOB;

    public GameObject object1_UI;
    public GameObject object1_OB;

    public GameObject object2_UI;
    public GameObject object2_OB;

    public GameObject object3_UI;
    public GameObject object3_OB;

    public GameObject object4_UI;
    public GameObject object4_OB;
    void Start()
    {
        flashLightON.SetActive(false);
        
    }

    void Update()
    {
        if(flashLightOB.activeInHierarchy)
        {
            flashLightON.SetActive(true);
            flashLightOFF.SetActive(false);
        }
        
        else
        {
            flashLightON.SetActive(false);
            flashLightOFF.SetActive(true);
        }

        if (object4_OB.activeInHierarchy)
        {
            object4_UI.SetActive(true);
        }
        else
        {
            object4_UI.SetActive(false);
        }


        if (object3_OB.activeInHierarchy)
        {
            object3_UI.SetActive(true);
        }
        else
        {
            object3_UI.SetActive(false);
        }


        if (object2_OB.activeInHierarchy)
        {
            object2_UI.SetActive(true);
        }
        else
        {
            object2_UI.SetActive(false);
        }


        if (object1_OB.activeInHierarchy)
        {
            object1_UI.SetActive(true);
        }
        else
        {
            object1_UI.SetActive(false);
        }


    }
}
