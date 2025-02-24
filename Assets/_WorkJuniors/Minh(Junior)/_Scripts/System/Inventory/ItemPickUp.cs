using UnityEngine;
public class ItemPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject equippedObject;
    [SerializeField] private GameObject pickUpText;
    [SerializeField] private GameObject EPrompt;

    public void Interact()
    {      
        if (EventBus.Instance == null)
        {
            Debug.LogError("EventBus instance is null!");
        }
        else
        {
            EventBus.Instance.PickUpItem(_itemData, equippedObject);
            EPrompt.SetActive(false);
        }
        Destroy(gameObject);
    }

    public GameObject InteractionText()
    {
        return pickUpText;
    }
    void OnDestroy()
    {
        if (pickUpText != null && pickUpText.activeSelf)
        {
            pickUpText.SetActive(false);
        }
    }
}
