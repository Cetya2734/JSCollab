using UnityEngine;

public class PlayerRespawnUIManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject escapeMenu;
    public PlayerRespawn respawnScript;

    public GameObject player; // Gán player chứa FPSController vào đây

    private MonoBehaviour fpsControllerScript;

    void Start()
    {
        if (deathScreen != null) deathScreen.SetActive(false);
        if (escapeMenu != null) escapeMenu.SetActive(false);

        if (player != null)
        {
            fpsControllerScript = player.GetComponent("FPSController") as MonoBehaviour;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapeMenu != null)
            {
                bool isActive = escapeMenu.activeSelf;
                escapeMenu.SetActive(!isActive);
                Time.timeScale = isActive ? 1f : 0f;

                if (fpsControllerScript != null)
                {
                    fpsControllerScript.enabled = isActive; // Tắt nếu mở menu
                }
            }
        }
    }

    public void ResumeGame()
    {
        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
            Time.timeScale = 1f;

            if (fpsControllerScript != null)
                fpsControllerScript.enabled = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Die()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0f;

            if (fpsControllerScript != null)
                fpsControllerScript.enabled = false;
        }
    }

    public void OnRespawnButtonPressed()
    {
        if (respawnScript != null)
        {
            Time.timeScale = 1f;
            respawnScript.Respawn();
        }

        if (deathScreen != null)
            deathScreen.SetActive(false);

        if (fpsControllerScript != null)
            fpsControllerScript.enabled = true;
    }
}

