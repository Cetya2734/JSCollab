﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initialPosition;
    private Vector3 swayOffset;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        swayOffset = Vector3.Lerp(swayOffset, finalPosition, Time.deltaTime * smoothAmount);

        if (transform.parent.gameObject.activeSelf) // Check if the weapon is active
        {
            transform.localPosition = initialPosition + swayOffset;
        }
    }
}
       