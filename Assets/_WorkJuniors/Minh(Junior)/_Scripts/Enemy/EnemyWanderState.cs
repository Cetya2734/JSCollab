using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : EnemyBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Vector3 startPoint;
    private readonly float wanderRadius;
    private readonly float patrolSpeed;

    public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRadius, float speed) : base(enemy, animator)
    {
        this.agent = agent;
        this.startPoint = enemy.transform.position;
        this.wanderRadius = wanderRadius;
        this.patrolSpeed = speed;
    }

    public override void OnEnter()
    {
       // agent.speed = patrolSpeed;
        animator.CrossFade(SwimmingHash, crossFadeDuration);
    }

    public override void Update()
    {
        if(HasReachedDestination())
        {
            // Finding a new direction to wander
            var randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += startPoint;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            var finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
    }

    bool HasReachedDestination()
    {
        return !agent.pathPending
               && agent.remainingDistance <= agent.stoppingDistance
               && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}