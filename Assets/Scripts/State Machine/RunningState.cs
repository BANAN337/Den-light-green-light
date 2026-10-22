using UnityEngine;
using UnityEngine.AddressableAssets;

namespace State_Machine
{
    public class RunningState : TargetState
    {
        private Transform _self;
        private Transform _end;
        
        private AudioClip _clip;

        private SmoothMotion _upAndDown;
        private SmoothMotion _scaling;
        
        private float _totalDuration;
        private float _speed = 50;

        private Vector2 _startingScale;
        private Vector2 _endingScale;

        private Vector2 _startingPosition;
        private Vector2 _endingPosition;
        private float _height = 1f;
        private float _duration;
        private float _currentStep = 1;
        private float _numberOfSteps = 20;

        public RunningState(Animator animator, AudioSource source, float duration, Transform selfTransform, Vector2 targetScale,
            Transform endTransform) : base(animator,source)
        {
            _totalDuration = duration / _speed;
            
            _duration = duration / _speed / _numberOfSteps;
            _self = selfTransform;
            _end = endTransform;

            _upAndDown = new SmoothMotion();
            _scaling = new SmoothMotion();

            _startingScale = _self.localScale;
            _endingScale = targetScale;

            _startingPosition = _self.position;
            _endingPosition = Vector2.Lerp(_self.position, endTransform.position, _currentStep / _numberOfSteps);

            _clip = AudioManager.Instance.GetClip(ClipIndex.TargetSteps);
        }

        public override void Enter()
        {
            PlayAnimation("Running");
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            if (_currentStep >= _numberOfSteps)
            {
                return;
            }
            _self.localScale = _scaling.StoppableInterpolateVector(_startingScale, _endingScale, _totalDuration);

            _self.position =
                _upAndDown.ParabolicInterpolateVector(_startingPosition, _endingPosition, _height, _duration);


            if (_upAndDown.RemainingDuration <= 0)
            {
                TargetAudioSource.Stop();
                TargetAudioSource.PlayOneShot(_clip);
                
                _currentStep++;
                _upAndDown.Stop();
                
                if (_currentStep >= _numberOfSteps)
                {
                    return;
                }

                _startingPosition = _self.position;

                var t = _currentStep / _numberOfSteps;
                _endingPosition = Vector2.Lerp(_startingPosition, _end.position, t);
            }
        }
    }
}