using System.Collections;
using UnityEngine;

public sealed class EffectManager : Singleton<EffectManager>
{
	[SerializeField] private GameObject HitEffect;
    [SerializeField] private GameObject KillLineEffect;

    private ObjectPool hitEffectPool;
    private ObjectPool killLineEffectPool;

	protected override void Awake()
	{
		base.Awake();
		Initialize();
	}

	private void Initialize ()
	{
		hitEffectPool = new ObjectPool(HitEffect, 5);
		killLineEffectPool = new ObjectPool(KillLineEffect, 4);
	}

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
		StartCoroutine(EffectReturnToPool(effect, killLineEffectPool));
	}

	private IEnumerator EffectReturnToPool(ParticleSystem effect, ObjectPool pool)
	{
		yield return new WaitForSeconds(effect.main.duration);
		pool.ReturnToPool(effect.gameObject);
	}
}
