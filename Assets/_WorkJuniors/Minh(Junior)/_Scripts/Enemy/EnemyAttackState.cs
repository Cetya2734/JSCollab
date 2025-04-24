using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Transform player;
    private bool hasReachedDestination;
    
    // SphereCast parameters
    private float attackRadius = 2f; // Radius of the SphereCast
    private float attackRange = 2f; // Range of the SphereCast
    private int attackDamage = 20; // Damage dealt by the attack
    
    private readonly float postAttackSpeed;
    private readonly float minPostAttackDistance;
    private readonly float maxPostAttackDistance;

    private readonly AudioClip attackSound;

    public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player, AudioClip attackSound, float speed, float minDistance, float maxDistance) : base(enemy, animator)
    {
        this.agent = agent;
        this.player = player;
        this.attackSound = attackSound;
        this.postAttackSpeed = speed;
        this.minPostAttackDistance = minDistance;
        this.maxPostAttackDistance = maxDistance;
    }

    public override void OnEnter()
    {
        agent.SetDestination(player.position);
        animator.CrossFade(AttackHash, crossFadeDuration);
        AudioManager.Instance.PlaySound(attackSound, enemy.transform.position);
        DelayAttack();
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
            //Debug.DrawLine(enemy.transform.position, enemy.transform.position + directionToPlayer * attackRange, Color.red, 5f);
            
            Debug.DrawRay(enemy.transform.position, directionToPlayer * attackRange, Color.blue, 3f);
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
    
    private void OnDrawGizmosSelected()
    {
        if (enemy == null || player == null) return;

        // Draw sphere at enemy position (attack starting point)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRadius);

        // Calculate direction to player
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;
    
        // Draw attack range (SphereCast path)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position + directionToPlayer * attackRange, attackRadius);

        // Draw a line from enemy to attack end position
        Gizmos.color = Color.green;
        Gizmos.DrawLine(enemy.transform.position, enemy.transform.position + directionToPlayer * attackRange);
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

    private void DelayAttack()
    {
        enemy.StartCoroutine(PerformDelayedAttack(0.5f));
    }
    
    private IEnumerator PerformDelayedAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;
        
        // Perform the SphereCast
        if (Physics.SphereCast(enemy.transform.position, attackRadius, directionToPlayer, out RaycastHit hit, attackRange))
        {
            //Debug.DrawLine(enemy.transform.position, enemy.transform.position + directionToPlayer * attackRange, Color.red, 5f);
            
            Debug.DrawRay(enemy.transform.position, directionToPlayer * attackRange, Color.blue, 3f);
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
    private IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    
        agent.speed = postAttackSpeed;
        animator.CrossFade(ChargingHash, crossFadeDuration);
        
        Vector3 randomDirection;
        Vector3 newDestination;
        bool validPositionFound = false;
        int attempts = 0;
        const int maxAttempts = 10; // Limit the number of attempts to find a valid position

        // Try to find a valid position within the min and max distance
        do
        {
            randomDirection = Random.insideUnitSphere * maxPostAttackDistance;
            randomDirection += player.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxPostAttackDistance, NavMesh.AllAreas))
            {
                newDestination = hit.position;
                float distanceToPlayer = Vector3.Distance(newDestination, player.position);

                // Check if the new destination is within the desired range
                if (distanceToPlayer >= minPostAttackDistance && distanceToPlayer <= maxPostAttackDistance)
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
            Time.deltaTime * 100f // Adjust rotation speed
        );
    }
}