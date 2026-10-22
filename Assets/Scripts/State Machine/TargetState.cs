using State_Machine;
using UnityEngine;

public abstract class TargetState : IState
{
    protected Animator TargetAnimator;
    protected AudioSource TargetAudioSource;

    public TargetState(Animator animator, AudioSource audioSource)
    {
        TargetAnimator = animator;
        TargetAudioSource = audioSource;
    }

    protected void PlayAnimation(string animationName)
    {
        TargetAnimator.Play(animationName);
    }
    
    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
}
