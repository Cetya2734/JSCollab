using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour
{
    public Transform topPosition;  // Điểm trên
    public Transform bottomPosition;  // Điểm dưới
    public float speed = 2f;  // Tốc độ di chuyển

    private bool isMovingUp = true;  // Trạng thái thang máy
    private bool isMoving = false;   // Kiểm tra xem thang máy có đang di chuyển không

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving) // Nếu người chơi bước vào và thang máy không di chuyển
        {
            isMovingUp = !isMovingUp; // Đảo hướng di chuyển
            StartCoroutine(MoveElevator(isMovingUp ? topPosition.position : bottomPosition.position));
        }
    }

    private IEnumerator MoveElevator(Vector3 target)
    {
        isMoving = true; // Đánh dấu là thang máy đang di chuyển

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        isMoving = false; // Thang máy đã đến nơi, cho phép kích hoạt lần sau
    }
}
