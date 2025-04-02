using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_LookController : MonoBehaviour
{
    //This script makes the camera follows the FPS player
    public GameObject playerArms;
    public GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.rotation = playerArms.transform.rotation;
    }
}
