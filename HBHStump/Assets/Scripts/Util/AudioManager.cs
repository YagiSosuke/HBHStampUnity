using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioManager() { if (!Instance) Instance = this; }

    [SerializeField] AudioSource audioSource;

    public void PlayBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlaySE(AudioClip clip) => audioSource.PlayOneShot(clip);
    public void Stop() => audioSource.Stop();
}
