using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : IState
{
    
    protected readonly Enemy enemy;
    protected readonly Animator animator;

    //Get animation hashes
    protected static readonly int IdleHash = Animator.StringToHash("FishIdle");
    protected static readonly int SwimmingHash = Animator.StringToHash("FishPatrolling");
    protected static readonly int ChargingHash = Animator.StringToHash("FishCharging");
    protected static readonly int AttackHash = Animator.StringToHash("FishAttack");
    protected static readonly int DeathHash = Animator.StringToHash("Death");
    
    protected const float crossFadeDuration = 0.2f;
    
    protected EnemyBaseState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void OnEnter()
    {
        // no op   
    }
    
    public virtual void Update()
    {   
       
        // no op   
    }

    public virtual void FixedUpdate()
    {
        // no op
    }

    public virtual void OnExit()
    {
        // no op
    }


}
