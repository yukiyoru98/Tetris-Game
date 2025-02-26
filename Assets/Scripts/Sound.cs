using System;
using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void Play(AudioClip clip, Action finishCallback)
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(Finish(finishCallback));
    }
    IEnumerator Finish(Action finishCallback)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
		finishCallback?.Invoke();
	}
}
