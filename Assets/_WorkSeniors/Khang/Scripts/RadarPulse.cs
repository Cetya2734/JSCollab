using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulse : MonoBehaviour
{
    [SerializeField] private Transform pulseTransform;
    private float range;
    private float rangeMax;

    private void Awake()
    {
        pulseTransform = transform.Find("Pulse");
        rangeMax = 300f;
    }

    private void Update()
    {
        float rangeSpeed = 150f;
        range += rangeSpeed * Time.deltaTime; 
        if(range > rangeMax)
        {
            range = 0f;
        }
        pulseTransform.localScale = new Vector3(range, range);
    }
}
