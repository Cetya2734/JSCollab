using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]

public class Enemy : MonoBehaviour
{
    [SerializeField, Self] private NavMeshAgent agent;
    [SerializeField, Self] private PlayerDetector playerDetector;
    [SerializeField, Child] private Animator animator;

    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float timeBetweenAttacks = 5f;
    
    private StateMachine stateMachine;

    public CountdownTimer attackTimer;
    
    private void OnValidate() => this.ValidateRefs();

    void Start()
    {
        attackTimer = new CountdownTimer(timeBetweenAttacks);
        stateMachine = new StateMachine();
        
        var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
        var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
        
        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => attackState.IsAttackComplete()));

        stateMachine.SetState(wanderState);
    }
    
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    void Update()
    {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime); // Update the cooldown timer
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    
    public void Attack()
    {
        if (attackTimer.IsRunning) return;
        attackTimer.Start();
        
    }
}
