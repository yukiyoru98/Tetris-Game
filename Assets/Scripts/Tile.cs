using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class Tile : MonoBehaviour
{
	[SerializeField] private Transform TileVisual;

	public const float TILE_HEIGHT = 1.0f;

	private const float FADE_OUT_RATE = 0.1f;
	private const float SHAKE_POWER = 0.05f;
	private const float SHAKE_DURATION = 0.3f;

	private void Awake()
	{
		if (TileVisual == null)
		{
			Debug.LogError($"TileVisual reference must be set in inspector.");
		}
	}

	public void Kill()
	{
		StartCoroutine(KillCoroutine());
	}

	private IEnumerator KillCoroutine()
	{
		if (TileVisual.TryGetComponent(out SpriteRenderer tileSprite))
		{
			while (tileSprite.color.a > 0)
			{
				Color tmpColor = tileSprite.color;
				tmpColor.a -= FADE_OUT_RATE;
				if (tmpColor.a < 0)
				{
					tmpColor.a = 0;
				}
				tileSprite.color = tmpColor;
				yield return null;
			}
		}

		Destroy(gameObject);
	}

	public void Shake()
	{
		StartCoroutine(ShakeCoroutine());
	}

	private IEnumerator ShakeCoroutine()
	{
		Vector3 startPos = TileVisual.localPosition;

		float beginTime = Time.time;
		float endTime = beginTime + SHAKE_DURATION;

		while (Time.time < endTime && TileVisual) // TileVisual becomes null if tile is killed
		{
			float dTime = endTime - Time.time;
			float shakeRange = SHAKE_POWER * dTime;
			Vector3 newPos = TileVisual.transform.localPosition;
			newPos.y = startPos.y + Random.Range(-shakeRange, shakeRange);
			TileVisual.transform.localPosition = newPos;
			yield return null;
		}

		if(TileVisual) // check existence in case tile is killed
		{
			TileVisual.transform.localPosition = startPos;
		}

	}
}
