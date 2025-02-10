using System.Collections;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;               // Tốc độ di chuyển của cá
    public Transform spawnArea;            // Khu vực di chuyển của cá
    private Vector2 targetPosition;        // Vị trí mục tiêu của cá

    private void Start()
    {
        // Tìm một vị trí ngẫu nhiên ngay khi bắt đầu
        SetRandomTargetPosition();
    }

    private void Update()
    {
        // Di chuyển cá tới vị trí mục tiêu
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Kiểm tra nếu cá đã đến gần vị trí mục tiêu
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Chọn vị trí ngẫu nhiên mới
            SetRandomTargetPosition();
        }
    }

    // Hàm chọn vị trí ngẫu nhiên trong khu vực spawn
    void SetRandomTargetPosition()
    {
        float x = Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2);
        float y = Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2);
        targetPosition = new Vector2(x, y);
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
