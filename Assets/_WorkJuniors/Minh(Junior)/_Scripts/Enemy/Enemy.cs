using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;  // Assign player in Inspector
    [SerializeField, Self] private NavMeshAgent agent;
    [SerializeField, Self] private PlayerDetector playerDetector;
    [SerializeField, Child] private Animator animator;

    [SerializeField] private float investigationDuration = 5f;

    private Vector3 lastKnownPlayerPosition;
    private CountdownTimer investigationTimer;
    
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float timeBetweenAttacks = 5f;

    [SerializeField] private AudioClip chargingSound;
    [SerializeField] private AudioClip detectedSound;
    [SerializeField] private AudioClip attackSound;
    
    private StateMachine stateMachine;

    public CountdownTimer attackTimer;
    private bool shouldStagger;
    
    public Vector3 LastDamageSource { get; private set; } // Store the last damage source position
    
    private void OnValidate() => this.ValidateRefs();

    void Start()
    {
        attackTimer = new CountdownTimer(timeBetweenAttacks);
        investigationTimer = new CountdownTimer(investigationDuration);
        stateMachine = new StateMachine();
        
        var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player, detectedSound, chargingSound);
        var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player, attackSound);
        var staggerState = new EnemyStaggerState(this, animator, agent);
        var investigateState = new EnemyInvestigateState(this, animator, agent);

        At(investigateState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(investigateState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        
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
    
    void Any(IState to, Func<bool> condition)
        => stateMachine.AddAnyTransition(to, new FuncPredicate(condition));
    
    void Update()
    {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime); 
        shouldStagger = false;
    }
    
    public void OnTakeDamage(Vector3 damageSource)
    {
        //CameraManager.Instance.ShakeCamera();
        LastDamageSource = damageSource; 
        shouldStagger = true;
        
        if (!playerDetector.CanDetectPlayer())
        {
            lastKnownPlayerPosition = player.position;
            agent.speed = 4f;
            agent.SetDestination(lastKnownPlayerPosition);
            investigationTimer.Tick(Time.deltaTime);
        }
    }
    
    public void OnInvestigationComplete()
    {
        stateMachine.SetState(new EnemyWanderState(this, animator, agent, wanderRadius));
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
