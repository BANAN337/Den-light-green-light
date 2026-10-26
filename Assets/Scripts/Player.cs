using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private ClipIndex[] indexes;
    
    private List<AudioClip> _clips;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomFootsteps()
    {
        AudioPlayer.PlayRandomClip(_audioSource, indexes);
    }
}
