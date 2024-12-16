using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    [SerializeField] private GameObject interactionRange;
    [SerializeField] private KeyCode interactionKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(interactionKey))
            {
                //Put a function inside the object that enables interactable bool
                //
                //call that function here 
            }

        }
    }

}
