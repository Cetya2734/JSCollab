using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotation Setting")] 
    [Range(1f,500f)]
    [SerializeField] private float _speed;
    [SerializeField] private bool _randomDirection;
    
    void Start()
    {
        if (_randomDirection)
        {
            int dir = Random.Range(0, 2);
            if (dir == 1)
                _speed += -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, _speed * Time.deltaTime);
    }
}
