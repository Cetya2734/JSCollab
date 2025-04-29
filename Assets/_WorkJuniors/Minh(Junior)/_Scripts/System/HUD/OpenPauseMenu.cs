using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                DisablePauseMenu();
            }
            else
            {
                SetPauseMenuActive();
            }
        }
    }

    public void SetPauseMenuActive()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }   
    public void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}