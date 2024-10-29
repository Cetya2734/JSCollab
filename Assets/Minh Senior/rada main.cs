
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radamain : MonoBehaviour
{
    public Transform player;
    public float radarRange = 10f;
    public GameObject radarPointPrefab;
    private List<GameObject> radarPoints = new List<GameObject>();

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, radarRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                GameObject radarPoint = radarPoints.Find(point => point.GetComponent<RadarPoint>().enemy == hit.transform);
                if (radarPoint == null)
                {
                    radarPoint = Instantiate(radarPointPrefab, transform);
                    radarPoint.GetComponent<RadarPoint>().enemy = hit.transform;
                    radarPoints.Add(radarPoint);
                }

                Vector3 direction = (hit.transform.position - player.position).normalized;
                radarPoint.transform.localPosition = direction * (radarRange / 2);
                Debug.Log("enemy");
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.white;  
            Gizmos.DrawWireSphere(player.position, radarRange);  
        }
    }
}

