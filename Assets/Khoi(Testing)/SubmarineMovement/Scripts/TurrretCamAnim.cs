using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrretCamAnim : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsLeft", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("IsLeft", true);
           
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("IsLeft", false);
           
        }
    }
}
