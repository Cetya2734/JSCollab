using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class EnemyStaggerState : EnemyBaseState
{
    private readonly NavMeshAgent agent;
    private readonly float staggerDuration = 0.5f;
    private readonly float knockbackDuration;
    private readonly float knockbackForce;
    
    private CountdownTimer staggerTimer;
    private CountdownTimer knockbackTimer;
    
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Vector3 knockbackDirection;

    public EnemyStaggerState(Enemy enemy, Animator animator, NavMeshAgent agent, float knockbackForce, float knockbackDuration) : base(enemy, animator)
    {
        this.agent = agent;
        this.knockbackForce = knockbackForce;
        this.knockbackDuration = knockbackDuration;
    }

    public override void OnEnter()
    {
        // Stop movement and store initial rotation
        agent.isStopped = true;
        initialRotation = enemy.transform.rotation;

        // Randomize rotation direction (left or right)
        float rotationAngle = Random.Range(15f, 30f) * (Random.Range(0, 2) * 2 - 1); // -30 to -45 or 30 to 45
        targetRotation = initialRotation * Quaternion.Euler(0, rotationAngle, 0);

        // Initialize timer
        staggerTimer = new CountdownTimer(staggerDuration);
        knockbackTimer = new CountdownTimer(knockbackDuration);

        staggerTimer.Start();
        knockbackTimer.Start();

        // Calculate knockback direction and force
        knockbackDirection = (enemy.transform.position - enemy.LastDamageSource).normalized;
    }

    public override void Update()
    {
        staggerTimer.Tick(Time.deltaTime);
        knockbackTimer.Tick(Time.deltaTime);
        
        // Apply knockback force
        if (!knockbackTimer.IsFinished)
        {
            agent.Move(knockbackDirection * knockbackForce * Time.deltaTime);
        }
        
        // Smoothly rotate toward target rotation
        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }

    public override void OnExit()
    {
        agent.isStopped = false;
    }

    public bool IsStaggerComplete() => staggerTimer.IsFinished;
}