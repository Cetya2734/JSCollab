using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject shockwavePrefab; // The shockwave prefab
    public Transform shockwaveSpawnPoint; // Point where shockwave will spawn

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerShockwave();
        }
    }

    void TriggerShockwave()
    {
        Instantiate(shockwavePrefab, shockwaveSpawnPoint.position, Quaternion.identity); //Spawn the shockwave prefab
    }
}
