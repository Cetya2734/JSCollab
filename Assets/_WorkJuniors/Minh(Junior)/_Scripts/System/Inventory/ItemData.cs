using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/Item")]

public class ItemData : ScriptableObject
{
    // Accessor for the item name
    public string Name => _name;
    
    // Accessor for the item description
    public List<string> Description => _description;
    
    // Accessor for the item sprite
    public Image Sprite => _sprite;

    // Flag to indicate equippable/consumables
    public bool IsEquippable => _isEquippable;

    public bool IsConsumable => _isConsumable;

    [SerializeField] private string _name;

    [SerializeField] private List<string> _description;

    [SerializeField] private Image _sprite;

    [SerializeField] private bool _isEquippable = false;

    [SerializeField] private bool _isConsumable = false;
}
