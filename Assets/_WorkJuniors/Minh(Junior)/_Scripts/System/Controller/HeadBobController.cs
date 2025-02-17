using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HeadBobController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _enable = true;
    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f;
    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
    }

    private void Update()
    {
        if (!_enable) return;
        
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion()
    {
        float verticalBob = Mathf.Sin(Time.time * _frequency) * _amplitude;
        float horizontalBob = Mathf.Cos(Time.time * _frequency / 2) * (_amplitude / 2);
        return new Vector3(horizontalBob, verticalBob, 0f);
    }

    private void CheckMotion()
    {
        float speed = _controller.velocity.magnitude;
        if (speed < _toggleSpeed || !_controller.isGrounded) return;
        
        PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        return transform.position + _cameraHolder.forward * 15.0f + Vector3.up * _cameraHolder.localPosition.y;
    }

    private void ResetPosition()
    {
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }
    
}
