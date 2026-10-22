using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ClipGetter<T> where T : UnityEngine.Object
{
    public static async Task<T> GetClip(string fileName)
    {
        var handle = Addressables.LoadAssetAsync<T>(fileName);
        
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        return null;
    }
}
