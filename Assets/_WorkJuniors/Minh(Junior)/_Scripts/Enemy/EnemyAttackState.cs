using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Transform player;
    private bool hasReachedDestination;
    
    // SphereCast parameters
    private float attackRadius = 2f; // Radius of the SphereCast
    private float attackRange = 3f; // Range of the SphereCast
    private int attackDamage = 20; // Damage dealt by the attack

    public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
    {
        this.agent = agent;
        this.player = player;
    }

    public override void OnEnter()
    {
        agent.SetDestination(player.position);
        animator.CrossFade(AttackHash, crossFadeDuration);
        PerformAttack();
        enemy.attackTimer.Reset(); // Reset the cooldown timer
        
        enemy.Attack(); // Start attack cooldown timer
        NewDestinationAfterAttack(); // Move to new position
        hasReachedDestination = false; // Reset destination flag

        // Ensure the agent is not stopped
        agent.isStopped = false;
    }
    public void PerformAttack()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;
        
        // Perform the SphereCast
        if (Physics.SphereCast(enemy.transform.position, attackRadius, directionToPlayer, out RaycastHit hit, attackRange))
        {
            Debug.DrawLine(enemy.transform.position, enemy.transform.position + directionToPlayer * attackRange, Color.red, 5f);
            // Check if the hit object is the player
            if (hit.transform == player)
            {
                // Apply damage to the player
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    public override void Update()
    {
        // Check if the enemy has reached the new destination
        if (!hasReachedDestination && HasReachedDestination())
        {
            hasReachedDestination = true;
            RotateTowardPlayer();
        }
    }

    private void NewDestinationAfterAttack()
    {
        enemy.StartCoroutine(MoveAfterDelay(0.6f)); // Adjust delay as needed
    }
    
    private IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    
        agent.speed = 2f; // Set a higher speed for the agent

        float minDistance = 5f; // Minimum distance from the player
        float maxDistance = 8f; // Maximum distance from the player

        Vector3 randomDirection;
        Vector3 newDestination;
        bool validPositionFound = false;
        int attempts = 0;
        const int maxAttempts = 10; // Limit the number of attempts to find a valid position

        // Try to find a valid position within the min and max distance
        do
        {
            randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += player.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                newDestination = hit.position;
                float distanceToPlayer = Vector3.Distance(newDestination, player.position);

                // Check if the new destination is within the desired range
                if (distanceToPlayer >= minDistance && distanceToPlayer <= maxDistance)
                {
                    agent.SetDestination(newDestination);
                    validPositionFound = true;
                    break;
                }
            }

            attempts++;
        } while (!validPositionFound && attempts < maxAttempts);

        if (!validPositionFound)
        {
            Debug.LogWarning("Failed to find a valid NavMesh position within the desired range");
        }
    }

    private bool HasReachedDestination()
    {
        return !agent.pathPending
               && agent.remainingDistance <= agent.stoppingDistance
               && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    public bool IsAttackComplete()
    {
        bool complete = hasReachedDestination && enemy.attackTimer.IsFinished;
        return complete;
    }
    
    private void RotateTowardPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;

        // Create a target rotation looking at the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate toward the player
        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            Time.deltaTime * 3f // Adjust rotation speed
        );
    }
}