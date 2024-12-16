using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomCursor : MonoBehaviour
{
    private Vector2 targetPos;
    
    
    void Start()
    {
        Cursor.visible = true;


       
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(targetPos);
        transform.position = targetPos;
    }
}
