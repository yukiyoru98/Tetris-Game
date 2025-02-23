using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	// Singletone
	private static EffectManager instance;
	public static EffectManager Instance
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

	private void Initialize ()
	{
		hitEffectPool = new ObjectPool(HitEffect, 5);
		killLineEffectPool = new ObjectPool(KillLineEffect, 4);
	}

	[SerializeField] private GameObject HitEffect;
    [SerializeField] private GameObject KillLineEffect;

    private ObjectPool hitEffectPool;
    private ObjectPool killLineEffectPool;

    public void ShowHitEffect(Vector3 pos)
    {
		ParticleSystem effect = hitEffectPool.GetFromPool().GetComponent<ParticleSystem>();
        effect.transform.position = pos;
		StartCoroutine(EffectReturnToPool(effect, hitEffectPool));
    }
    public void ShowKillLineEffect(Vector3 pos)
    {
        ParticleSystem effect = killLineEffectPool.GetFromPool().GetComponent<ParticleSystem>();
		effect.transform.position = pos;
		StartCoroutine(EffectReturnToPool(effect, hitEffectPool));
	}

	private IEnumerator EffectReturnToPool(ParticleSystem effect, ObjectPool pool)
	{
		yield return new WaitForSeconds(effect.main.duration);
		pool.ReturnToPool(effect.gameObject);
	}
}
