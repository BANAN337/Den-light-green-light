using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ClipGetter
{
    public static async Task<AudioClip> GetClip(string clipName)
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>(clipName);
        
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            return null;
        }
    }
}
