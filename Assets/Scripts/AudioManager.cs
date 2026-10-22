using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum ClipIndex
{
    TargetSteps,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    private Dictionary<ClipIndex, AudioClip> _clipsDictionary = new();
    [SerializeField] private string[] databaseName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
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
            }
        }
    }

    public AudioClip GetClip(ClipIndex clipIndex)
    {
        return _clipsDictionary.GetValueOrDefault(clipIndex);
    }
}
