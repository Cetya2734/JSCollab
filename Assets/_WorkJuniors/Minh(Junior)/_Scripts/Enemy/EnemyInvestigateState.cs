using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInvestigateState : EnemyBaseState
{
    private readonly Enemy enemy;
    private readonly Animator animator;
    private readonly NavMeshAgent agent;

    public EnemyInvestigateState(Enemy enemy, Animator animator, NavMeshAgent agent) : base(enemy, animator)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.agent = agent;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            enemy.OnInvestigationComplete();
        }
    }
}