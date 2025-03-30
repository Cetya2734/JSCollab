using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private List<ItemData> possibleLoot;
    [SerializeField] private GameObject lootPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnRandomLoot();
            Destroy(gameObject);
        }
    }

    private void SpawnRandomLoot()
    {
        if (possibleLoot.Count == 0 || lootPrefab == null) return;

        ItemData lootItem = possibleLoot[Random.Range(0, possibleLoot.Count)];

        GameObject lootInstance = Instantiate(lootPrefab, transform.position + Vector3.up, Quaternion.identity);
        CollectibleLoot loot = lootInstance.GetComponent<CollectibleLoot>();
        loot.SetItemData(lootItem);
    }
}
