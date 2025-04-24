// using UnityEngine;
//
// public class HorizontalElevator : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     [SerializeField] private Transform startPoint;
//     [SerializeField] private Transform endPoint;
//     [SerializeField] private float moveSpeed = 2f;
//     [SerializeField] private bool loopMovement = false;
//
//     private bool isMoving = false;
//     private bool movingToEnd = true;
//     private Vector3 currentTarget;
//     void Update()
//     {
//         if (!isMoving) return;
//
//         MoveElevator();
//         UpdateTarget();
//     }
//
//     private void MoveElevator()
//     {
//         float step = moveSpeed * Time.deltaTime;
//         transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);
//     }
//
//     private void UpdateTarget()
//     {
//         if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
//         {
//             if (loopMovement)
//             {
//                 movingToEnd = !movingToEnd;
//                 currentTarget = movingToEnd ? endPoint.position : startPoint.position;
//             }
//             else
//             {
//                 isMoving = false;
//             }
//         }
//     }
//
//     public void ToggleElevator()
//     {
//         if (!isMoving)
//         {
//             currentTarget = movingToEnd ? endPoint.position : startPoint.position;
//             isMoving = true;
//         }
//         else if (!loopMovement)
//         {
//             isMoving = false;
//         }
//     }
//
//     public void SetMovementState(bool shouldMove)
//     {
//         isMoving = shouldMove;
//         if (shouldMove)
//         {
//             currentTarget = movingToEnd ? endPoint.position : startPoint.position;
//         }
//     }
// }

using UnityEngine;
using System.Collections.Generic;

public class HorizontalElevator : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool loopMovement = false;

    private bool isMoving = false;
    private bool movingToEnd = true;
    private Vector3 currentTarget;
    private Vector3 previousPosition;
    private List<CharacterController> passengers = new List<CharacterController>();

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 positionBeforeMove = transform.position;
        MoveElevator();
        UpdateTarget();

        // Calculate movement delta
        Vector3 delta = transform.position - positionBeforeMove;

        // Move passengers
        foreach (CharacterController passenger in passengers)
        {
            passenger.Move(delta);
        }

        previousPosition = transform.position;
    }

    private void MoveElevator()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);
    }

    private void UpdateTarget()
    {
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (loopMovement)
            {
                movingToEnd = !movingToEnd;
                currentTarget = movingToEnd ? endPoint.position : startPoint.position;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    public void ToggleElevator()
    {
        if (!isMoving)
        {
            currentTarget = movingToEnd ? endPoint.position : startPoint.position;
            isMoving = true;
        }
        else if (!loopMovement)
        {
            isMoving = false;
        }
    }

    public void SetMovementState(bool shouldMove)
    {
        isMoving = shouldMove;
        if (shouldMove)
        {
            currentTarget = movingToEnd ? endPoint.position : startPoint.position;
        }
    }

    // Add passengers when they enter the trigger
    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && !passengers.Contains(cc))
        {
            passengers.Add(cc);
        }
    }

    // Remove passengers when they exit the trigger
    private void OnTriggerExit(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && passengers.Contains(cc))
        {
            passengers.Remove(cc);
        }
    }
}