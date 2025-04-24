using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Transform player;
    private readonly AudioClip chargingSound; // Store the sound
    private readonly AudioClip detectedSound; // Store the sound
    private bool hasPlayedJumpscare = false;
    private readonly float chaseSpeed;


    public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player, AudioClip detectedSound, AudioClip chargingSound, float speed ) : base(enemy, animator)
    {
        this.agent = agent ;
        this.player = player;
        this.chargingSound = chargingSound;
        this.detectedSound = detectedSound;
        this.chaseSpeed = speed;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        agent.speed = chaseSpeed;
        animator.CrossFade(ChargingHash, crossFadeDuration);
        
        if (!hasPlayedJumpscare && detectedSound != null)
        {
            AudioManager.Instance.PlaySound(detectedSound, enemy.transform.position);
            hasPlayedJumpscare = true;
        }
        AudioManager.Instance.PlaySound(chargingSound, enemy.transform.position);
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
    }
    
    public override void OnExit()
    {
       agent.ResetPath(); // Clears the current destination
    }
}

