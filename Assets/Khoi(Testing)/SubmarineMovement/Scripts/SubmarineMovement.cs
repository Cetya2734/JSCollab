using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    [SerializeField] private float xThrust;
    [SerializeField] private float yThrust;

    [SerializeField] private int xNotch;
    [SerializeField] private int yNotch;
    [SerializeField] private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        ChangeNotch();
        XThrustSpeed();
        YThrustSpeed();
    }

    void XThrustSpeed()
    {
        rb.AddForce(new Vector2(xNotch, 0) * xThrust, ForceMode2D.Force);

    }
    void YThrustSpeed()
    {
        rb.AddForce(new Vector2(0, yNotch) * yThrust, ForceMode2D.Force);

    }









    void ChangeNotch()
    {
        //X Notch
        if (Input.GetKeyDown(KeyCode.A) && xNotch > -2)
        {
            xNotch -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D) && xNotch < 2)
        {
            xNotch += 1;
        }
        //Y Notch
        if (Input.GetKeyDown(KeyCode.S) && yNotch > -2)
        {
            yNotch -= 1;
        }
        if (Input.GetKeyDown(KeyCode.W) && yNotch < 2)
        {
            yNotch += 1;
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            yNotch = 0;
            xNotch = 0;
        }
    }
}
