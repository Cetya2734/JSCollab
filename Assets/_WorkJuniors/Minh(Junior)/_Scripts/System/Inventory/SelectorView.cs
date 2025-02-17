using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorView : MonoBehaviour
{
    private RectTransform _rectTransform;
    private GameObject _selected;
    [SerializeField] private float _speed = 25f;

    [SerializeField] private GameObject _starterSelectedObject;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetSlot();
    }

    void Update()
    {
        // Get the currently selected GameObject in the EventSystem
        var selectedGameObject = EventSystem.current.currentSelectedGameObject;

        // Update the reference to the selected GameObject
        _selected = (selectedGameObject == null) ? _selected : selectedGameObject;

        // Set the selected GameObject in the EventSystem
        EventSystem.current.SetSelectedGameObject(_selected);

        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null) return;

        // Smoothly move the SelectorView towards the position of the selected GameObject
        transform.position = Vector3.Lerp(transform.position, selected.transform.position, _speed * Time.deltaTime);

        var otherRect = selected.GetComponent<RectTransform>();

        // Smoothly adjust the size of the SelectorView to match the size of the selected GameObject
        var horizontalLerp = Mathf.Lerp(_rectTransform.rect.size.x, otherRect.rect.size.x, _speed * Time.deltaTime);
        var verticalLerp = Mathf.Lerp(_rectTransform.rect.size.y, otherRect.rect.size.y, _speed * Time.deltaTime);

        // Set the size of the SelectorView based on the interpolation
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalLerp);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalLerp);
    }

    private void SetSlot()
    {
        // Use the starter selected GameObject if provided
        if (_starterSelectedObject != null)
        {
            _selected = _starterSelectedObject;
            EventSystem.current.SetSelectedGameObject(_selected);
        }
    }
}
