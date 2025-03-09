using System;
using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource SoundSource;
    public void Play(AudioClip clip, Action finishCallback)
    {
        SoundSource.clip = clip;
        SoundSource.Play();
        StartCoroutine(Finish(finishCallback));
    }
    IEnumerator Finish(Action finishCallback)
    {
        yield return new WaitWhile(() => SoundSource.isPlaying);
        finishCallback?.Invoke();
    }
}
