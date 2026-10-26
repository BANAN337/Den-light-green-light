
using System.Collections;
using UnityEngine;
using IState = State_Machine.IState;

public class AttackModeState : TargetState
{
    public bool IsPressed { get; private set; }
    
    private Controller _controller;

    public AttackModeState(Animator animator, AudioSource source, Controller controller) : base(animator, source)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        PlayAnimation("Attack");
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (_controller.IsInputPressed)
        {
            IsPressed = true;
        }
    }
}
