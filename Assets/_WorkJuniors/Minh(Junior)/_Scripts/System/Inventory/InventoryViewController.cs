using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryViewController : MonoBehaviour
{
    [FormerlySerializedAs("_inventoryViewObject")] [SerializeField] private GameObject inventoryViewObject;

    [FormerlySerializedAs("_contextMenuObject")] [SerializeField] private GameObject contextMenuObject;

    [FormerlySerializedAs("_firstContextMenuOption")] [SerializeField] private GameObject firstContextMenuOption;

    [FormerlySerializedAs("_secondContextMenuOption")] [SerializeField] private GameObject secondContextMenuOption;

    [FormerlySerializedAs("_firstInventoryOption")] [SerializeField] private GameObject firstInventoryOption;

    [FormerlySerializedAs("_inspectMenu")] [SerializeField] private GameObject inspectMenu;

    [FormerlySerializedAs("_itemNameText")] [SerializeField] private TMP_Text itemNameText;

    [FormerlySerializedAs("_itemDescriptionText")] [SerializeField] private TMP_Text itemDescriptionText;
    
    [FormerlySerializedAs("_slots")] [SerializeField] private List<ItemSlot> slots;

    [FormerlySerializedAs("_currentSlot")] [SerializeField] private ItemSlot currentSlot;

    [FormerlySerializedAs("_currentlyEquippedItem")] [SerializeField] private GameObject currentlyEquippedItem;

    [FormerlySerializedAs("_fader")] [SerializeField] private ScreenFader fader;

    [FormerlySerializedAs("_contextMenuIgnore")] [SerializeField] private List<Button> contextMenuIgnore;

    [FormerlySerializedAs("_equipButton")] [SerializeField] private Button equipButton;

    [FormerlySerializedAs("_useButton")] [SerializeField] private Button useButton;

    [FormerlySerializedAs("_inspectButton")] [SerializeField] private Button inspectButton;

    [FormerlySerializedAs("_discardButton")] [SerializeField] private Button discardButton;

   // [SerializeField] private AudioSource footStepAudio;

    [FormerlySerializedAs("_dictionary")] [SerializeField] private ItemDictionary dictionary;

    [SerializeField] private float fadeDuration = 0.1f;
    private enum State
    {
        MenuClosed,

        MenuOpen,

        ContextMenu, 
    };

    private State _state;

       
    private void Start()
    {
        inspectMenu.transform.localScale = new Vector3(0, 0, 0);
    }
    
    private void Update()
    {
        // Toggle the inventory view 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_state == State.MenuClosed)
            {
                EventBus.Instance.PauseGameplay();
                fader.FadeToBlack(fadeDuration, FadeToMenuCallback);
                _state = State.MenuOpen;
                EventSystem.current.SetSelectedGameObject(firstInventoryOption);
            }
            // Close the inventory view
            else if (_state == State.MenuOpen)
            {
                fader.FadeToBlack(fadeDuration, FadeFromMenuCallback);
                _state = State.MenuClosed;
            }
            else if (_state == State.ContextMenu)
            {
                inspectMenu.transform.localScale = new Vector3(0, 0, 0);
                contextMenuObject.SetActive(false);
                foreach (var button in contextMenuIgnore)
                {
                    button.interactable = true;
                }
                EventSystem.current.SetSelectedGameObject(currentSlot.gameObject);
                _state = State.MenuOpen;
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject != null && selectedObject.TryGetComponent<Button>(out Button button))
            {
                button.onClick.Invoke(); // Simulate button click
            }
        }
        //Open context menu
        if (Input.GetButtonDown("Interact"))
        {
            if (_state == State.MenuOpen)
            {
                if (EventSystem.current.currentSelectedGameObject.TryGetComponent<ItemSlot>(out var slot))
                {
                    //Ensure the slot has an item before opening the menu
                    if (slot.itemData == null)
                        return;
                    
                    currentSlot = slot;
                    _state = State.ContextMenu;
                    contextMenuObject.SetActive(true);
                    
                    foreach (var button in contextMenuIgnore)
                    {
                        button.interactable = false;
                    }

                    if (currentSlot.itemData == null)
                    {
                        equipButton.interactable = false;
                        useButton.interactable = false;
                        inspectButton.interactable = false;
                        discardButton.interactable = false;
                    }
                    else
                    {
                        equipButton.interactable = currentSlot.itemData.IsEquippable;
                        useButton.interactable = currentSlot.itemData.IsConsumable;
                        
                        if (equipButton.interactable)
                        {
                            EventSystem.current.SetSelectedGameObject(firstContextMenuOption);
                        }
                        
                        if (useButton.interactable)
                        {
                            EventSystem.current.SetSelectedGameObject(secondContextMenuOption);
                        }
                    
                        inspectButton.interactable = true;
                        discardButton.interactable = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_state == State.ContextMenu)
                {
                    contextMenuObject.SetActive(false);
                    foreach (var button in contextMenuIgnore)
                    {
                        button.interactable = true;
                    }
                }
            }
        }
    }
    
    public void UseItem()
    {
        fader.FadeToBlack(fadeDuration,FadeToUseItemCallback );
    }
    public void FadeToUseItemCallback()
    {
        contextMenuObject.SetActive(false);
        inventoryViewObject.SetActive(false);
        
        itemNameText.text = "";
        itemDescriptionText.text = "";
        
        foreach (var button in contextMenuIgnore)
        {
            button.interactable = true;
        }
        
        EventSystem.current.SetSelectedGameObject(currentSlot.gameObject);
        
        fader.FadeFromBlack(fadeDuration, () =>
        {
            EventBus.Instance.UseItem(currentSlot.itemData);
            ClearItemData();
        });
        
        _state = State.MenuClosed;
        
        EventBus.Instance.ResumeGameplay();
    }

    public void EquipItem()
    {
        fader.FadeToBlack(fadeDuration, FadeToEquipItemCallback);
    }

    public void FadeToEquipItemCallback()
    {
        contextMenuObject.SetActive(false);
        inventoryViewObject.SetActive(false);

        if (currentlyEquippedItem != null)
        {
            currentlyEquippedItem.SetActive(false);
        }
        
        GameObject equipObject = dictionary.GetEquipObjectForPickUp(currentSlot.itemData);
        
        foreach (var button in contextMenuIgnore)
        {
            button.interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(currentSlot.gameObject);

        if (currentSlot.itemData.IsEquippable)
        {
            fader.FadeFromBlack(fadeDuration, () =>
            {
                if (equipObject != null)
                {
                    equipObject.SetActive(true);
                    currentSlot.equippedObject = equipObject;
                    currentlyEquippedItem = equipObject;
                }
                else
                {
                    Debug.Log("No equippable item");
                }
            });
            
            _state = State.MenuClosed;
        }
        EventBus.Instance.ResumeGameplay();
    }

    public void Inspect()
    {
        inspectMenu.transform.localScale = new Vector3(1, 1, 1);
    }
    
    public void OnSlotSelected(ItemSlot selectedSlot)
    {
        // Check if the selected slot is empty
        if (selectedSlot.itemData == null)
        {
            currentSlot = null;
            // Clear the item name and description texts if the slot is empty
            itemNameText.ClearMesh();
            itemDescriptionText.ClearMesh();
            return;
        }
        currentSlot = selectedSlot;
        // Display the name and first description of the selected item in UI
        itemNameText.SetText(selectedSlot.itemData.name);
        itemDescriptionText.SetText(selectedSlot.itemData.Description[0]);
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
        foreach (var slot in slots)
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
 
    // ReSharper disable Unity.PerformanceAnalysis
    private void FadeToMenuCallback()
    {
        fader.FadeFromBlack(fadeDuration, null);
        inventoryViewObject.SetActive(true);
    }
    private void FadeFromMenuCallback()
    {
        inventoryViewObject.SetActive(false);
        fader.FadeFromBlack(fadeDuration, EventBus.Instance.ResumeGameplay);
    }

    public void ClearItemData()
    {
        currentSlot.ClearItemData();
    }
    
    public bool HasItem(ItemData item)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item)
            {
                return true;
            }
        }
        return false;
    }
}
