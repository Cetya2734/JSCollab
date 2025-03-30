using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(ItemData item)
    {
        if (itemNameText) itemNameText.text = item.Name;
        if (itemDescriptionText && item.Description.Count > 0) itemDescriptionText.text = item.Description[0];
        if (itemImage && item.Sprite != null) itemImage.sprite = item.Sprite.sprite;
    }
}
