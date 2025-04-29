using UnityEngine;
using UnityEngine.SceneManagement; // If you're using a separate scene
using UnityEngine.UI; // If you're using a UI Panel

public class ControlsMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    public Button backButton; 

    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBackToMainMenu);
        }
        else
        {
            Debug.LogError("Back button is not assigned!");
        }
    }

    void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

   
}