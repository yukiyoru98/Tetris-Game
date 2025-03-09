using System.Collections.Generic;
using UnityEngine;

public sealed class SFXPlayer : Singleton<SFXPlayer>
{
    [SerializeField] private AudioClip[] Clips;
    [SerializeField] private GameObject SoundPrefab;

    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    private ObjectPool sfxPool;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        sfxPool = new ObjectPool(SoundPrefab, 1);
        foreach (AudioClip clip in Clips)
        {
            clipDictionary[clip.name] = clip;
        }
    }

    public void PlaySFX(string clipName)
    {
        if (clipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            Sound sound = sfxPool.GetFromPool().GetComponent<Sound>();
            sound.Play(clip, () => sfxPool.ReturnToPool(sound.gameObject));
        }

    }

}
