using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchingScript : MonoBehaviour
{
    [SerializeField] private Camera turretCamera;
    [SerializeField] private Camera sonarCamera;
    // Start is called before the first frame update
    void Start()
    {
        turretCamera.depth = 1;
        sonarCamera.depth = 0;
        Test.instance.ChangeCamera(turretCamera);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turretCamera.depth = 1;
            sonarCamera.depth = 0;
            Test.instance.ChangeCamera(turretCamera);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            turretCamera.depth = 0;
            sonarCamera.depth = 1;
            Test.instance.ChangeCamera(sonarCamera);
        }
    }
}
