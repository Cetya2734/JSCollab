using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerRespawn player = other.GetComponent<PlayerRespawn>();
        if (player != null)
        {
            player.UpdateSafePosition(other.transform.position);
        }
    }
}