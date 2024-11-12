using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffect : MonoBehaviour
{
    [SerializeField] private float scaleSpeed;

    [SerializeField] private float minScale, maxScale;

    private Vector2 scale;
    
    private void OnEnable()
    {
        scale = transform.localScale;
    }
    
    private void OnDisable()
    {
        transform.localScale = new Vector2(0.15f, 0.15f);
    }

    void Update()
    {
        scale = new Vector2(Mathf.Clamp(scale.x += Time.deltaTime * scaleSpeed, minScale, maxScale), scale.y);
        scale = new Vector2(scale.x, Mathf.Clamp(scale.y += Time.deltaTime * scaleSpeed, minScale, maxScale));
        transform.localScale = scale;
    }
}
