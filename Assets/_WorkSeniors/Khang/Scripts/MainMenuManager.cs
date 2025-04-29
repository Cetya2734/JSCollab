using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "GameScene"; //replace with name of actual game scene
    public AudioSource buttonClickSound;
    public GameObject quitConfirmationPanel;
    public GameObject soundSettingsPanel;

    public void PlayGame()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenControls()
    {
        Debug.Log("Controls button clicked!");
    }

    public void OpenSounds()
    {
        Debug.Log("Sounds button clicked!");
        soundSettingsPanel.SetActive(true);
    }

    public void ShowQuitConfirmation()
    {
        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Quit Confirmation Panel is not assigned in the Inspector!");
        }
    }

    public void QuitGameYes()
    {
        Debug.Log("Quit confirmed!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void QuitGameNo()
    {
        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }
}
