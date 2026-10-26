using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum ClipIndex
{
    TargetSteps1,
    TargetSteps2,
    TargetSteps3,
    TargetSteps4,
    NormalFootsteps1,
    NormalFootsteps2,
    NormalFootsteps3,
    NormalFootsteps4,
    NormalFootsteps5,
    MenacingRunning,
    Jumpscare
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public Action OnClipsLoaded { get; set; }
    
    [SerializeField] private string[] databaseName;
    
    private Dictionary<ClipIndex, AudioClip> _clipsDictionary = new();
    private bool _isLoaded;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        var tasks = new List<Task<AudioDatabase>>();
        
        foreach (var data in databaseName)
        {
            tasks.Add(ClipGetter<AudioDatabase>.GetClip(data));
        }
        
        var allClips = await Task.WhenAll(tasks);

        foreach (var element in allClips)
        {
            foreach (var clipData in element.audioClips)
            {
                _clipsDictionary.Add(clipData.index, clipData.audioClip);
                //Debug.Log(clipData.index + " : " + clipData.audioClip.name);
            }
        }
        _isLoaded = true;
        OnClipsLoaded?.Invoke();
    }

    public AudioClip GetClip(ClipIndex clipIndex)
    {
        return _isLoaded ? _clipsDictionary.GetValueOrDefault(clipIndex) : null;
    }
}
