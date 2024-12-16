using UnityEngine;

public class Fish : MonoBehaviour
{
    public int fishValue = 10; // Giá trị của cá
    private bool isCaught = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hook") && !isCaught)
        {
            isCaught = true;
            CatchFish();
        }
    }

    void CatchFish()
    {
        Debug.Log("Caught a fish worth " + fishValue + " points!");

        // Tìm FishSpawner trong scene và gọi hàm FishCaught
        FishSpawner spawner = FindObjectOfType<FishSpawner>();
        if (spawner != null)
        {
            spawner.FishCaught(gameObject);
        }

        Destroy(gameObject); // Hoặc disable gameObject
    }
}
