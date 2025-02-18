using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryViewController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryViewObject;

    [SerializeField] private GameObject _contextMenuObject;

    [SerializeField] private GameObject _firstContextMenuOption;

    [SerializeField] private GameObject _secondContextMenuOption;

    [SerializeField] private GameObject _firstInventoryOption;

    [SerializeField] private GameObject _inspectMenu;

    [SerializeField] private TMP_Text _itemNameText;

    [SerializeField] private TMP_Text _itemDescriptionText;

   // [SerializeField] private FPSController _characterController;

    [SerializeField] private List<ItemSlot> _slots;

    [SerializeField] private ItemSlot _currentSlot;

    [SerializeField] private GameObject _currentlyEquippedItem;

    [SerializeField] private ScreenFader _fader;

    [SerializeField] private List<Button> _contextMenuIgnore;

    [SerializeField] private Button _equipButton;

    [SerializeField] private Button _useButton;

    [SerializeField] private Button _inspectButton;

    [SerializeField] private Button _discardButton;

   // [SerializeField] private AudioSource footStepAudio;

    [SerializeField] private ItemDictionary _dictionary;

    [SerializeField] private float fadeDuration = 0.1f;
    private enum State
    {
        menuClosed,

        menuOpen,

        contextMenu, 
    };

    private State _state;

       
    private void Start()
    {
        //footStepAudio = footStepAudio.GetComponent<AudioSource>();
        _inspectMenu.transform.localScale = new Vector3(0, 0, 0);
    }
    
    private void Update()
    {
        // Toggle the inventory view on/off when the Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_state == State.menuClosed)
            {
                //EventBus.Instance.PauseGameplay();
                _fader.FadeToBlack(fadeDuration, FadeToMenuCallback);
                _state = State.menuOpen;
                EventSystem.current.SetSelectedGameObject(_firstInventoryOption);
            }
            else if (_state == State.menuOpen)
            {
                _fader.FadeToBlack(fadeDuration, FadeFromMenuCallback);
                _state = State.menuClosed;
            }
            else if (_state == State.contextMenu)
            {
                _inspectMenu.transform.localScale = new Vector3(0, 0, 0);
                _contextMenuObject.SetActive(false);
                foreach (var button in _contextMenuIgnore)
                {
                    button.interactable = true;
                }
                EventSystem.current.SetSelectedGameObject(_currentSlot.gameObject);
                _state = State.menuOpen;
            }
        }

        //Open context menu
        if (Input.GetButtonDown("Interact"))
        {
            if (_state == State.menuOpen)
            {
                if (EventSystem.current.currentSelectedGameObject.TryGetComponent<ItemSlot>(out var slot))
                {
                    //Ensure the slot has an item before opening the menu
                    if (slot.itemData == null)
                        return;
                    
                    _currentSlot = slot;
                    _state = State.contextMenu;
                    _contextMenuObject.SetActive(true);
                    
                    foreach (var button in _contextMenuIgnore)
                    {
                        button.interactable = false;
                    }

                    if (_currentSlot.itemData == null)
                    {
                        _equipButton.interactable = false;
                        _useButton.interactable = false;
                        _inspectButton.interactable = false;
                        _discardButton.interactable = false;
                    }
                    else
                    {
                        _equipButton.interactable = _currentSlot.itemData.IsEquippable;
                        _useButton.interactable = _currentSlot.itemData.IsConsumable;
                        
                        if (_equipButton.interactable)
                        {
                            EventSystem.current.SetSelectedGameObject(_firstContextMenuOption);
                        }
                        
                        if (_useButton.interactable)
                        {
                            EventSystem.current.SetSelectedGameObject(_secondContextMenuOption);
                        }
                    
                        _inspectButton.interactable = true;
                        _discardButton.interactable = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_state == State.contextMenu)
                {
                    _contextMenuObject.SetActive(false);
                    foreach (var button in _contextMenuIgnore)
                    {
                        button.interactable = true;
                    }
                }
            }
        }
    }
    
    public void UseItem()
    {
        _fader.FadeToBlack(fadeDuration,FadeToUseItemCallback );
    }
    public void FadeToUseItemCallback()
    {
        _contextMenuObject.SetActive(false);
        _inventoryViewObject.SetActive(false);
        
        _itemNameText.text = "";
        _itemDescriptionText.text = "";
        
       // footStepAudio.volume = 1f;
        
        foreach (var button in _contextMenuIgnore)
        {
            button.interactable = true;
        }
        
        EventSystem.current.SetSelectedGameObject(_currentSlot.gameObject);
        
        _fader.FadeFromBlack(fadeDuration, () =>
        {
            EventBus.Instance.UseItem(_currentSlot.itemData);
            ClearItemData();
        });
        
        _state = State.menuClosed;
        
        //.Instance.ResumeGameplay();
    }

    public void EquipItem()
    {
        _fader.FadeToBlack(fadeDuration, FadeToEquipItemCallback);
    }

    public void FadeToEquipItemCallback()
    {
        _contextMenuObject.SetActive(false);
        _inventoryViewObject.SetActive(false);

        if (_currentlyEquippedItem != null)
        {
            _currentlyEquippedItem.SetActive(false);
        }
        
        GameObject equipObject = _dictionary.GetEquipObjectForPickUp(_currentSlot.itemData);
        //footStepAudio.volume = 1f;
        
        foreach (var button in _contextMenuIgnore)
        {
            button.interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(_currentSlot.gameObject);

        if (_currentSlot.itemData.IsEquippable)
        {
            _fader.FadeFromBlack(fadeDuration, () =>
            {
                if (equipObject != null)
                {
                    equipObject.SetActive(true);
                    _currentSlot.equippedObject = equipObject;
                    _currentlyEquippedItem = equipObject;
                }
                else
                {
                    Debug.Log("No equippable item");
                }
            });
            
            _state = State.menuClosed;
        }
        //EventBus.Instance.ResumeGameplay();
    }

    public void Inspect()
    {
        _inspectMenu.transform.localScale = new Vector3(1, 1, 1);
    }
    
    public void OnSlotSelected(ItemSlot selectedSlot)
    {
        // Check if the selected slot is empty
        if (selectedSlot.itemData == null)
        {
            _currentSlot = null;
            // Clear the item name and description texts if the slot is empty
            _itemNameText.ClearMesh();
            _itemDescriptionText.ClearMesh();
            return;
        }
        _currentSlot = selectedSlot;
        // Display the name and first description of the selected item in UI
        _itemNameText.SetText(selectedSlot.itemData.name);
        _itemDescriptionText.SetText(selectedSlot.itemData.Description[0]);
    }


    // Subscribe and Unsubscribe to the onPickUpItem event in EventBus
    private void OnEnable()
    {
        EventBus.Instance.onPickUpItem += OnItemPickedUp;
    }

    private void OnDisable()
    {
        EventBus.Instance.onPickUpItem -= OnItemPickedUp;
    }

    private void OnItemPickedUp(ItemData itemData, GameObject equippedObject)
    {
        // Find the first empty slot and assign the picked-up item to it
        foreach (var slot in _slots)
        {
            if (slot == null)
            {
                Debug.LogError("Found a null slot in the slots list!");
                continue;
            }
            
            if (slot.IsEmpty())
            {
                slot.itemData = itemData;
                slot.equippedObject = equippedObject;
                break;
            }
        }
    }
 
    private void FadeToMenuCallback()
    {
        _inventoryViewObject.SetActive(true);
        //footStepAudio.volume = 0f;
        _fader.FadeFromBlack(fadeDuration,  null);
    }
    private void FadeFromMenuCallback()
    {
        _inventoryViewObject.SetActive(false);
       // footStepAudio.volume = 10f;
        //_fader.FadeFromBlack(0.3f, EventBus.Instance.ResumeGameplay);
        _fader.FadeFromBlack(fadeDuration, null);
    }

    public void ClearItemData()
    {
        _currentSlot.ClearItemData();
    }
}
