using System.Collections;
using UnityEngine;

namespace State_Machine
{
    public class TurningState : TargetState
    {
        public TurningState(Animator animator,AudioSource source) : base(animator,source)
        {
            
        }

        public override void Enter()
        {
            PlayAnimation("Turning");
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }
    }
}
