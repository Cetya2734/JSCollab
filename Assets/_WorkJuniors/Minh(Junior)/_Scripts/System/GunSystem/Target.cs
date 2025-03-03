using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // [SerializeField] private GameObject OriginalOB;
    // [SerializeField] private GameObject ChangeOB;
    // [SerializeField] private Animator ANI;
    
    public float health = 100f;

    // Flags to control different actions upon taking damage
    public bool animate;
    public bool replace;
    public bool destroy;

    public void TakeDamage(float amount)
    {
        health -= amount;
        // if(health <= 0f && animate)
        // {
        //     Animate();
        // }
        //
        // if (health <= 0f && replace)
        // {
        //     Replace();
        // }

        if (health <= 0f)
        {
            Destroy();
        }
    }

    // void Animate()
    // {
    //     ANI.SetBool("animate", true);
    // }


    // void Replace()
    // {
    //     Debug.Log(OriginalOB);
    //     OriginalOB.SetActive(false);
    //     ChangeOB.SetActive(true);
    // }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
