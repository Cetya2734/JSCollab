using UnityEngine;
public class ItemPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject equippedObject;
    [SerializeField] private GameObject pickUpText;

    public void Interact()
    {
        EventBus.Instance.PickUpItem(_itemData, equippedObject);
        Destroy(gameObject);
    }

    public GameObject GetGameObject()
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
