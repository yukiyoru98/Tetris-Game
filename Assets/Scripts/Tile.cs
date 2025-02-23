using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //[SerializeField] private ParticleSystem _hitEffect;

    //[SerializeField] private AudioSource _hitSource;
    public void Kill()
    {
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    {
        float diff = 0.1f;
        while (_spriteRenderer.color.a > 0)
        {
           Color tmpColor = _spriteRenderer.color;
            tmpColor.a -= diff;
            if (tmpColor.a < 0) tmpColor.a = 0;
            _spriteRenderer.color = tmpColor;
            yield return null;
        }
        Destroy(gameObject);
    }

    public IEnumerator Shake()
    {
        var beginTime = Time.realtimeSinceStartup;
        var beginY = _spriteRenderer.transform.localPosition.y;
        var endTime = beginTime + 0.3f;

        var spriteTransform = _spriteRenderer.transform;

        while (Time.realtimeSinceStartup < endTime && spriteTransform)
        {
            var diffTime = endTime - Time.realtimeSinceStartup;
            var range = 0.05f * diffTime;
            var pos = spriteTransform.localPosition;
            pos.y = beginY + Random.Range(-range, range);
            spriteTransform.localPosition = pos;
            yield return null;
        }
        if(spriteTransform)
        {
            var pos = spriteTransform.localPosition;
            pos.y = beginY;
            spriteTransform.localPosition = pos;
        }

    }
}
