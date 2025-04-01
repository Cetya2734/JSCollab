using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    public GameObject deathScreen; // UI khi chết
    public GameObject escapeMenu;  // Panel khi nhấn ESC

    void Start()
    {
        respawnPoint = transform.position; // Lưu vị trí ban đầu
        if (deathScreen != null) deathScreen.SetActive(false);
        if (escapeMenu != null) escapeMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Nhấn ESC
        {
            if (escapeMenu != null)
            {
                bool isActive = escapeMenu.activeSelf;
                escapeMenu.SetActive(!isActive);
                Time.timeScale = isActive ? 1f : 0f; // Dừng hoặc tiếp tục game
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeRoom"))
        {
            respawnPoint = other.transform.position; // Cập nhật điểm hồi sinh
        }
    }
    public void ResumeGame()
    {
        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Die()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
        deathScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
