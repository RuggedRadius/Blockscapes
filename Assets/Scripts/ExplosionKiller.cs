using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionKiller : MonoBehaviour
{
    public float delay;
    public float fadeOutDuration;
    private bool fading;

    private float initialLightRange;

    void Update()
    {
        delay -= Time.deltaTime;

        if (delay <= 0f)
        {
            if (!fading)
            {
                StartCoroutine(fade(fadeOutDuration));
            }
        }
    }

    private IEnumerator fade(float duration)
    {
        fading = true;

        Light light = this.GetComponent<Light>();
        initialLightRange = light.range;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            light.range = Mathf.Lerp(initialLightRange, 0f, timer / duration);
            yield return null;
        }

        Destroy(this.gameObject);
        yield return null;
    }
}
