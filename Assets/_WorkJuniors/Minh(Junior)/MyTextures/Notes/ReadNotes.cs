using UnityEngine;

public class ReadNotes : MonoBehaviour, IInteractable
{
    public GameObject player;
    public GameObject noteUI;
    public GameObject hud;
    public GameObject inv;

    public GameObject pickUpText;

    public AudioSource pickUpSound;
    

    public void Interact()
    {
        EventBus.Instance.PauseGameplay();
        noteUI.SetActive(true);
        pickUpSound.Play();
        hud.SetActive(false);
        inv.SetActive(false);
        player.GetComponent<CharacterController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public GameObject InteractionText()
    {
        return pickUpText;
    }

    void Start()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
        pickUpText.SetActive(false);
    }

    public void ExitButton()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
        EventBus.Instance.ResumeGameplay();
    }

}
