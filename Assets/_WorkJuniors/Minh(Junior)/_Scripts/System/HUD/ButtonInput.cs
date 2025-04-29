using UnityEngine;
using UnityEngine.UI;

public class ButtonInput : MonoBehaviour
{
    public Button myButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            myButton.onClick.Invoke();  // Simulate the button being clicked
        }
    }
}