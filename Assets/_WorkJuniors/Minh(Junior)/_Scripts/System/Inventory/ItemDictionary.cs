using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ItemPairs
{
    public ItemData pickUpItemData;
    public GameObject equipObject;
}

public class ItemDictionary : MonoBehaviour
{
    [SerializeField] private List<ItemPairs> _pickUpEquipPairs = new List<ItemPairs>();

    // Method to add a pick-up and equip object pair
    public void AddPair(ItemData pickUpItemData, GameObject equipObject)
    {
        _pickUpEquipPairs.Add(new ItemPairs { pickUpItemData = pickUpItemData, equipObject = equipObject });
    }

    // Method to remove a pick-up and equip object pair
    public void RemovePair(ItemData pickUpItemData)
    {
        _pickUpEquipPairs.RemoveAll(pair => pair.pickUpItemData == pickUpItemData);
    }

    // Method to find the equip object corresponding to a pick-up object
    public GameObject GetEquipObjectForPickUp(ItemData pickUpItemData)
    {
        foreach (var pair in _pickUpEquipPairs)
        {
            if (pair.pickUpItemData == pickUpItemData)
            {
                return pair.equipObject;
            }
        }
        return null; // Return null if no corresponding equip object found
    }
}


