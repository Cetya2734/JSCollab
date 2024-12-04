using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab; // Prefab của cá
    public Transform spawnArea; // Khu vực spawn cá
    public int maxFishCount = 10; // Số lượng cá tối đa
    private int currentFishCount = 0; // Số lượng cá hiện tại
    private List<GameObject> spawnedFishes = new List<GameObject>(); // Danh sách cá đã được tạo

    private void Start()
    {
        StartCoroutine(SpawnFish());
    }

    IEnumerator SpawnFish()
    {
        while (currentFishCount < maxFishCount) // Chỉ tạo cá nếu chưa đạt tối đa
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2)
            );

            GameObject fish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
            spawnedFishes.Add(fish);
            currentFishCount++;

            yield return new WaitForSeconds(Random.Range(1f, 3f)); // Tạo cá ngẫu nhiên sau mỗi khoảng thời gian
        }
    }

    // Hàm để giảm số lượng cá khi cá bị bắt
    public void FishCaught(GameObject fish)
    {
        if (spawnedFishes.Contains(fish))
        {
            spawnedFishes.Remove(fish);
            currentFishCount--;
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnArea.position, spawnArea.localScale);
        }
    }

}
