using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private MonoBehaviour[] scriptsToEnable;
    [SerializeField] private CharacterActions characterActions;

    public void ResumeGame()
    {
        // Resume time
        Time.timeScale = 1;

        // Hide the pause menu
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        // Re-enable gameplay-related scripts
        foreach (var script in scriptsToEnable)
        {
            if (script != null)
                script.enabled = true;
        }

        // Re-enable CharacterActions script
        if (characterActions != null)
            characterActions.enabled = true;

        // Lock and hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
