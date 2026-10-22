using System.Collections;
using UnityEngine;

namespace State_Machine
{
    public class AnticipationState : TargetState
    {
        private float _maxDuration;
        private float _minDuration;
        private StateSwitcher _stateSwitcher;

        public AnticipationState(Animator animator, AudioSource source) : base(animator, source)
        {

        }
        
        public override void Enter()
        {
            
            PlayAnimation("Anticipation");
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
