using System;
using System.Collections;
using State_Machine;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private Transform objectToMove;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 targetScale = new(7.3f, 7.3f);
    [SerializeField] private Controller controller;

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
        StartCoroutine(SwitchToAnticipationState(2));
    }

    private void AiLoop()
    {
    }

    private IEnumerator SwitchToAnticipationState(float delay)
    {
        _stateSwitcher.SwitchState(new AnticipationState(_animator,_audioSource));
        yield return new WaitForSeconds(delay);
        StartCoroutine(SwitchToTurningState(delay));
    }

    private IEnumerator SwitchToTurningState(float delay)
    {
        _stateSwitcher.SwitchState(new TurningState(_animator,_audioSource));
        yield return new WaitForSeconds(delay);
        StartCoroutine(SwitchToAttackModeState(delay));
    }

    private IEnumerator SwitchToAttackModeState(float delay)
    {
        var attackModeState = new AttackModeState(_animator,_audioSource, controller);
        _stateSwitcher.SwitchState(attackModeState);
        yield return new WaitForSeconds(delay);

        if (attackModeState.IsPressed)
        {
            controller.DisableControls();
            StartCoroutine(SwitchToRunningState(delay, duration));
        }
        else
        {
            StartCoroutine(SwitchToAnticipationState(delay));
        }
    }

    private IEnumerator SwitchToRunningState(float delay, float runningDuration)
    {
        _stateSwitcher.SwitchState(new RunningState(_animator,_audioSource, runningDuration, transform, targetScale, objectToMove));
        yield return new WaitForSeconds(delay);
    }

    public void ContinueMoving()
    {
        transform.position = _smoothPosition.StoppableInterpolateVector(_startPosition, objectToMove.position, duration);
        transform.localScale = _smoothScaling.StoppableInterpolateVector(_startScale, targetScale, duration);
    }

    public void Stop()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
        duration = _smoothPosition.RemainingDuration;

        _smoothPosition.Stop();
        _smoothScaling.Stop();
    }
}