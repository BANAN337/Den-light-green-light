using System.Collections;
using State_Machine;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private float duration = 100;
    private float _startingDuration;
    private float _firstDivider = 0.25f;

    private float Duration
    {
        get => duration;
        set
        {
            if (value <= _startingDuration / 3)
            {
                minAnticipationTime = minAnticipationTime * _firstDivider;
                minTurningTime = minTurningTime * _firstDivider;
                minAttackModeTime = minAttackModeTime * _firstDivider;
            }
            else if (value <= _startingDuration /4)
            {
                maxAnticipationTime = maxAnticipationTime * _firstDivider;
                maxTurningTime = maxTurningTime * _firstDivider;
                maxAttackModeTime = maxAttackModeTime * _firstDivider;
            }
            duration = value;
        }
    }
    
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Vector2 targetScale = new(7.3f, 7.3f);
    [SerializeField] private Controller controller;
    [SerializeField] private ClipIndex[] clipIndexes;
    
    [Space]
    [SerializeField] private float minAnticipationTime = 3.5f;
    [SerializeField] private float maxAnticipationTime = 7f;
    [SerializeField] private float minTurningTime = 0.9f;
    [SerializeField] private float maxTurningTime = 1.2f;
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
        transform.position = _smoothPosition.StoppableInterpolateVector(_startPosition, objectToMove.position, Duration);
        transform.localScale = _smoothScaling.StoppableInterpolateVector(_startScale, targetScale, Duration);
    }

    public void Stop()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
        Duration = _smoothPosition.RemainingDuration;

        _smoothPosition.Stop();
        _smoothScaling.Stop();
    }
}