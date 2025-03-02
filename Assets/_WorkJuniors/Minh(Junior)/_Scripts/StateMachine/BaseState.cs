using UnityEngine;

public abstract class BaseState : IState
{

    protected readonly PlayerController player;
    protected readonly Animator animator;
    protected const float crossFadeDuration = 0.1f;
    
    protected BaseState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }
    public void OnEnter()
    {
        //null
    }

    public void Update()
    {
        //null
    }

    public void FixedUpdate()
    {
        //null
    }

    public void OnExit()
    {
        //null
    }
}