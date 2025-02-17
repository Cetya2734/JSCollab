using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject cube;

    public string GetInteractionPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        // Get the Renderer component from the new cube
        var cubeRenderer = cube.GetComponent<Renderer>();

        // Create a new RGBA color using the Color constructor and store it in a variable
        Color customColor = new Color(0.4f, 0.9f, 0.7f, 1.0f);

        // Call SetColor using the shader property name "_Color" and setting the color to the custom color you created
        cubeRenderer.material.SetColor("_Color", customColor);
    }

    public GameObject GetGameObject()
    {
        return null;
    }
}

