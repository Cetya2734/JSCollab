using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private KeyCode continueKey;
    private bool isPaused;

    
    // Start is called before the first frame update
    void Start()
    {
        textBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(continueKey) && isPaused == true)
        {
            Debug.Log("unPause");
            Time.timeScale = 1;
            textBox.SetActive(false);
            isPaused = false;

        }
    }

    void ShowText()
    {      
        textBox.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Player"))
        {
            ShowText();
        }
    }
}
