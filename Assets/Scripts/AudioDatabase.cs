using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum ClipIndex
{
    TargetSteps,
    
}

public class AudioDatabase : MonoBehaviour
{
    public Dictionary<ClipIndex, AudioClip> ClipsDictionary { get; private set; }= new();
    [SerializeField] private AudioClipData[] audioClipsData;

    private async void Start()
    {
        var tasks = new List<Task<AudioClip>>();
        
        foreach (var data in audioClipsData)
        {
            tasks.Add(ClipGetter.GetClip(data.clipName));
        }
        
        var allClips = await Task.WhenAll(tasks);

        for (var i = 0; i < allClips.Length; i++)
        {
            ClipsDictionary[audioClipsData[i].index] = allClips[i];
        }

    }
}
