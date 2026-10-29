using System;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private AudioSource _audioSource;
    private ColorfulParticles[] _particles;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _particles = GetComponentsInChildren<ColorfulParticles>();
        _audioSource.PlayOneShot(clip);
    }

    private void Start()
    {
        foreach (var particle in _particles)
        {
            particle.PlayConfetti();
        }
    }
}
