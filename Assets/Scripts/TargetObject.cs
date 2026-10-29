using System;
using System.Collections;
using State_Machine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private float duration = 100;
    private float _startingDuration;
    private float _firstDivider = 0.25f;
    private event Action OnThresholdReached;

    private float Duration
    {
        get => duration;
        set
        {
            if (value <=0)
            {
                controller.DisableControls();
                _animator.Play("Ending");
                SceneSwitcher.LoadScene(SceneIndex.End);
            }
            OnThresholdReached?.Invoke();
            
            duration = value;
        }
    }
    
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Vector2 targetScale = new(7.3f, 7.3f);
    [SerializeField] private Controller controller;
    [SerializeField] private ClipIndex[] clipIndexes;
    
    [Space]
    [SerializeField] private float minAnticipationTime = 2.5f;
    [SerializeField] private float maxAnticipationTime = 7f;
    [SerializeField] private float minTurningTime = 0.9f;
    [SerializeField] private float maxTurningTime = 1f;
    [SerializeField] private float minAttackModeTime = 1f;
    [SerializeField] private float maxAttackModeTime = 4f;

    private Animator _animator;
    private AudioSource _audioSource;

    private SmoothMotion _smoothPosition;
    private SmoothMotion _smoothScaling;
    private Vector2 _startPosition;
    private Vector2 _startScale;

    private StateSwitcher _stateSwitcher;

    private void Awake()
    {
        _smoothPosition = new SmoothMotion();
        _smoothScaling = new SmoothMotion();
        _startPosition = transform.position;
        _startScale = transform.localScale;
        _startingDuration = duration;

        _stateSwitcher = new StateSwitcher(new AnticipationState(_animator,_audioSource));

        OnThresholdReached += OnFirstThresholdReached;
        OnThresholdReached += OnSecondThresholdReached;
        
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _stateSwitcher.CurrentState.Update();
    }

    private void Start()
    {
        StartCoroutine(SwitchToAnticipationState());
    }

    private void OnFirstThresholdReached()
    {
        if (Duration <= _startingDuration * 3 / 4)
        {
            maxAnticipationTime *= _firstDivider;
            maxTurningTime = 0.6f;
            maxAttackModeTime *= _firstDivider;
            OnThresholdReached -= OnFirstThresholdReached;
        }
    }
    private void OnSecondThresholdReached()
    {
        if (Duration <= _startingDuration / 3)
        {
            minAnticipationTime *= _firstDivider;
            minTurningTime *= 0.2f;
            minAttackModeTime *= _firstDivider;
            OnThresholdReached -= OnSecondThresholdReached;
        }
    }
    

    private IEnumerator SwitchToAnticipationState()
    {
        var delay = Random.Range(minAnticipationTime, maxAnticipationTime);
        _stateSwitcher.SwitchState(new AnticipationState(_animator,_audioSource));
        yield return new WaitForSeconds(delay);
        StartCoroutine(SwitchToTurningState());
    }

    private IEnumerator SwitchToTurningState()
    {
        var delay = Random.Range(minTurningTime, maxTurningTime);
        _stateSwitcher.SwitchState(new TurningState(_animator,_audioSource));
        yield return new WaitForSeconds(delay);
        StartCoroutine(SwitchToAttackModeState());
    }

    private IEnumerator SwitchToAttackModeState()
    {
        var delay = Random.Range(minAttackModeTime, maxAttackModeTime);
        var attackModeState = new AttackModeState(_animator,_audioSource, controller);
        _stateSwitcher.SwitchState(attackModeState);
        yield return new WaitForSeconds(delay);

        if (attackModeState.IsPressed)
        {
            controller.DisableControls();
            StopAllCoroutines();
            StartCoroutine(SwitchToRunningState(Duration));
        }
        else
        {
            StartCoroutine(SwitchToAnticipationState());
        }
    }

    private IEnumerator SwitchToRunningState(float runningDuration)
    {
        var runningState =
            new RunningState(_animator, _audioSource, runningDuration, transform, targetScale, objectToMove);
        _stateSwitcher.SwitchState(runningState);
        yield return new WaitForSeconds(runningState.TotalDuration);
        StartCoroutine(SwitchToJumpscareState(1));
    }

    private IEnumerator SwitchToJumpscareState(float delay)
    {
        _stateSwitcher.SwitchState(new JumpscareState(_animator, _audioSource, transform, objectToMove, delay));
        yield return new WaitForSeconds(delay);
        _stateSwitcher.CurrentState.Exit();
    }

    public void ContinueMoving()
    {
        transform.position = _smoothPosition.StoppableInterpolateVector(_startPosition, objectToMove.position, Duration, Stop);
        transform.localScale = _smoothScaling.StoppableInterpolateVector(_startScale, targetScale, Duration);
    }

    public void Stop()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
        Duration = Mathf.Clamp(_smoothPosition.RemainingDuration, 0, _startingDuration);

        _smoothPosition.Stop();
        _smoothScaling.Stop();
    }

    
}