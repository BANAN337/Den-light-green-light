using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[RequireComponent(typeof(ParticleSystem))]
public class ColorfulParticles : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private readonly Color[] _colors = { Color.blue, Color.yellow, Color.red, Color.green, Color.cyan, Color.magenta, };
    private ParticleSystem.MainModule _mainModule;
    public ParticleSystem ParticleSystem => _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _mainModule = _particleSystem.main;
    }

    private void Update()
    {
        RandomColor();
    }

    private void RandomColor()
    {
        if (_particleSystem.isPlaying)
            _mainModule.startColor = _colors[UnityEngine.Random.Range(0, _colors.Length)];
    }

    public void PlayConfetti()
    {
        _particleSystem.Play(true);
    }

    public void Stop()
    {
        _particleSystem.Stop(true);
    }
}