using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject gameObject1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetActiveObject()
    {
        if (gameObject1.activeSelf)
        {
            gameObject1.SetActive(false);
        }
        else
        {
            gameObject1.SetActive(true);
        }
    }
}
