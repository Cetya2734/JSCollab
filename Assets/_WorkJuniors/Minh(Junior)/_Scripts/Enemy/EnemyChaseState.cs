using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Transform player;

    public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player ) : base(enemy, animator)
    {
        this.agent = agent ;
        this.player = player;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        agent.speed = 4f;
        animator.CrossFade(ChargingHash, crossFadeDuration);
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

