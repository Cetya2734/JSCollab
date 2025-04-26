using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CharacterActions characterActions;

    private void OnEnable()
    {
        EventBus.Instance.onItemUsed += HandleItemUsed;
    }

    private void OnDisable()
    {
        EventBus.Instance.onItemUsed -= HandleItemUsed;
    }

    private void HandleItemUsed(ItemData itemData)
    {
        switch (itemData.Type)
        {
            case ItemType.Medkit:
                playerHealth.RestoreHealth(itemData.Potency);
                break;

            case ItemType.AmmoPack:
                characterActions.AddAmmo((int)itemData.Potency);
                break;
        }
    }
}