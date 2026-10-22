using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Database", menuName = "Audio Database")]
public class AudioDatabase : ScriptableObject
{
    public AudioClipData[] audioClips;
}
