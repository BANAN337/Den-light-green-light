using UnityEngine;

namespace DefaultNamespace
{
    public static class AudioPlayer
    {
        public static AudioClip PlayRandomClip(AudioSource audioSource, ClipIndex[] indexes)
        {
            audioSource.Stop();
            var index = Random.Range(0, indexes.Length);
            var clip = AudioManager.Instance.GetClip(indexes[index]);
            audioSource.PlayOneShot(clip);
            return clip;
        }

        public static AudioClip PlayClip(AudioSource audioSource, ClipIndex index)
        {
            var clip = AudioManager.Instance.GetClip(index);
            audioSource.PlayOneShot(clip);
            return clip;
        }
    }
}