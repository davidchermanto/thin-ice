using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;

public class Hit_Light : MonoBehaviour
{
    [SerializeField] private Light2D glow;

    private float inTime = 0.15f;
    private float inTime2 = 2f;

    private float fadeTime = 1.2f;
    private float deleteTime = 2f;

    private float scaleUp = 2.4f;

    private float initialIntensity = 150;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));

        StartCoroutine(SizeIn());
        StartCoroutine(Fade());
        StartCoroutine(Delete());
    }

    private IEnumerator SizeIn()
    {
        float time = 0;

        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = new Vector3(scaleUp, scaleUp);

        while(time < 1)
        {
            time += Time.deltaTime / inTime;

            transform.localScale = Vector3.Lerp(currentScale, targetScale, time);

            yield return new WaitForEndOfFrame();
        }

        time = 0;
        currentScale = transform.localScale;
        targetScale = new Vector3(scaleUp * 1.5f, scaleUp * 1.5f);

        while (time < 1)
        {
            time += Time.deltaTime / inTime2;

            transform.localScale = Vector3.Lerp(currentScale, targetScale, time);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Fade()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / fadeTime;

            glow.intensity = Mathf.Lerp(initialIntensity, 0, time);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Delete()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / deleteTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
