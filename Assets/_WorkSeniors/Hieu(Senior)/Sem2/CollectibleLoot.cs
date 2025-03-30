using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLoot : MonoBehaviour
{
    private ItemData itemData;
    [SerializeField] private GameObject lootPopupPrefab;

    public void SetItemData(ItemData data)
    {
        itemData = data;

        // Optional: change mesh material or color based on item type
        // GetComponent<MeshRenderer>().material = itemData.GetMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            EventBus.Instance.PickUpItem(itemData, null);

            if (lootPopupPrefab != null)
            {
                GameObject canvas = GameObject.Find("HUD"); // your Canvas in the scene
                GameObject popup = Instantiate(lootPopupPrefab, canvas.transform); // set as child of Canvas
                popup.GetComponent<LootPopup>().ShowPopup(itemData);
            }

            Destroy(gameObject);
        }
    }
}
