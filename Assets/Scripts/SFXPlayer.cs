using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    private static SFXPlayer instance;
    public static SFXPlayer Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

	private void Awake()
    {
		if (Instance != null && Instance != this)
		{
			Debug.LogError("Duplicated Singleton.");
			Destroy(Instance);
			return;
		}
		Instance = this;
		Initialize();

	}

    [SerializeField] private AudioClip[] Clips;
	[SerializeField] private GameObject SoundPrefab;

    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    private ObjectPool sfxPool;

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
		if(clipDictionary.TryGetValue(clipName, out AudioClip clip))
		{
			Sound sound = sfxPool.GetFromPool().GetComponent<Sound>();
			sound.Play(clip, () => sfxPool.ReturnToPool(sound.gameObject));
		}

	}

}
