using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LookAtObjects : MonoBehaviour,IInteractable
{
    public TextMeshProUGUI textOB;
    public GameObject gameObjects;
    public string description = "Description";

    private WaitForSeconds interactionDelay = new WaitForSeconds(1f);

    public void Interact()
    {
        textOB.text = description.ToString();
        textOB.enabled = true;

        StartCoroutine(DisableTextAfterDelay());
    }

    public GameObject InteractionText()
    {
        return gameObjects;
    }

    private IEnumerator DisableTextAfterDelay()
    {
        // Wait for the specified duration
        yield return interactionDelay;

        // Disable the textOB
        textOB.enabled = false;
    }
    
    void Start()
    {
        textOB.enabled = false;
    }
}
