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
    private bool shouldStagger;
    
    public Vector3 LastDamageSource { get; private set; } // Store the last damage source position

    
    private void OnValidate() => this.ValidateRefs();

    void Start()
    {
        attackTimer = new CountdownTimer(timeBetweenAttacks);
        stateMachine = new StateMachine();
        
        var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
        var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
        var staggerState = new EnemyStaggerState(this, animator, agent);
        
        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => attackState.IsAttackComplete()));

        Any(staggerState, () => shouldStagger);
        At(staggerState, wanderState, new FuncPredicate(() => staggerState.IsStaggerComplete() && !playerDetector.CanDetectPlayer()));
        At(staggerState, chaseState, new FuncPredicate(() => staggerState.IsStaggerComplete() && playerDetector.CanDetectPlayer()));

        
        stateMachine.SetState(wanderState);
    }
    
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    // void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    
    void Any(IState to, Func<bool> condition)
        => stateMachine.AddAnyTransition(to, new FuncPredicate(condition));
    
    void Update()
    {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime); // Update the cooldown timer
        shouldStagger = false; // Reset flag after processing
    }
    
    public void OnTakeDamage(Vector3 damageSource)
    {
        LastDamageSource = damageSource; // Store the damage source position
        shouldStagger = true;
        Debug.Log("Staggered");
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
