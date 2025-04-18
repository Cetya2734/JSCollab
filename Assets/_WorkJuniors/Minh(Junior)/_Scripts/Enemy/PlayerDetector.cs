using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayerDetector : MonoBehaviour
{
    // Detection cone
    [SerializeField] private float detectionAngle = 60f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] float innerDetectionRadius = 5f;
    [SerializeField] private float detectionCooldown = 1f;
    [SerializeField] private float attackRange = 0.5f;

    public Transform Player { get; private set; }
    CountdownTimer detectionTimer;

    IDetectionStrategy detectionStrategy;

    void Start()
    {
        detectionTimer = new CountdownTimer(detectionCooldown);

        if (FPSController.Instance != null)
        {
            Player = FPSController.Instance.transform;
        }
        else
        {
            Debug.LogError("FPSController not found");
            enabled = false;
            return;
        }
        
        detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        detectionTimer.Tick(Time.deltaTime);
        var directionToPlayer = Player.position - transform.position;
//        Debug.Log(directionToPlayer.magnitude);
    }

    public bool CanDetectPlayer()
    {
        return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);
    }

    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;

    public bool CanAttackPlayer()
    {
        var directionToPlayer = Player.position - transform.position;
        return directionToPlayer.magnitude <= attackRange;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
        Vector3 backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
        Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
    }
}

