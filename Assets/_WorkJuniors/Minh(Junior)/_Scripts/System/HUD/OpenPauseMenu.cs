using System;
using UnityEngine;

public class OpenPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private CharacterActions characterActions;

    void Start()
    {
        if (pauseMenu == null)
            pauseMenu = GameObject.Find("PauseMenu");

        if (characterActions == null)
        {
            characterActions = GetComponentInChildren<CharacterActions>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
                DisablePauseMenu();
            else
                SetPauseMenuActive();
        }
    }

    public void SetPauseMenuActive()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (characterActions != null)
            characterActions.enabled = false;
    }

    public void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (characterActions != null)
            characterActions.enabled = true;
    }
}
