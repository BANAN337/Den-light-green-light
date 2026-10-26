using DefaultNamespace;
using UnityEngine;

public class JumpscareState : TargetState
{
    private SmoothMotion _movement = new();
    private SmoothMotion _volume = new();
    private SmoothMotion _scale = new();

    private Transform _self;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    
    private float _startVolume;
    
    private Vector2 _startScale;
    private Vector2 _endScale;

    private float _duration;

    public JumpscareState(Animator animator, AudioSource audioSource, Transform selfTransform, Transform endTransform, float duration) :
        base(animator, audioSource)
    {
        _self = selfTransform;

        _startPosition = selfTransform.position;
        _endPosition = new Vector2(0,0);
        
        _startVolume = TargetAudioSource.volume;
        
        _startScale = selfTransform.localScale;
        _endScale = new Vector2(30,30);
        
        _duration = duration;
    }

    public override void Enter()
    {
        PlayAnimation("Jumpscare");
        AudioPlayer.PlayClip(TargetAudioSource, ClipIndex.Jumpscare);
        SceneSwitcher.LoadScene(SceneIndex.Main);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        _self.position = _movement.StoppableInterpolateVector(_startPosition, _endPosition, _duration);
        _self.localScale = _scale.StoppableInterpolateVector(_startScale, _endScale, _duration);
        TargetAudioSource.volume = _volume.InterpolateFloat(_startVolume, 0, _duration + SceneSwitcher.FadeTime);
    }
}