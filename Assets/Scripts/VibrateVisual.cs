using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateVisual : MonoBehaviour
{
    [SerializeField] private OreCollider ore;
    [SerializeField] private SpriteRenderer main;
    [SerializeField] private Transform rotateTransform;
    [SerializeField] private SpriteRenderer orb1;
    [SerializeField] private SpriteRenderer orb2;
    [SerializeField] private SpriteRenderer orb3;
    [SerializeField] private SpriteRenderer orb4;

    private float rotationPerSecond = 240;
    private float targetOpacity = 0.3f;
    private float opacityShiftTime = 0.4f;
    private float opacityFadeTime = 0.4f;

    // scale = 0.24 per 0.5
    // scale = 0.47 per 1

    // Start is called before the first frame update
    public void Initialize(float scale, OreCollider ore, bool inRange)
    {
        this.ore = ore;
        float newScale = scale * 0.47f;
        transform.localScale = new Vector3(newScale, newScale);

        if (inRange)
        {
            main.color = new Color(0.5f, 1f, 0.5f);
            orb1.color = new Color(0.5f, 1f, 0.5f);
            orb2.color = new Color(0.5f, 1f, 0.5f);
            orb3.color = new Color(0.5f, 1f, 0.5f);
            orb4.color = new Color(0.5f, 1f, 0.5f);
        }

        StartCoroutine(RotateOrbs());

        OpacityUp();
        ore.visualActive = true;
    }

    public void OpacityUp()
    {
        StartCoroutine(ShiftOpacity());
    }

    private IEnumerator RotateOrbs()
    {
        while (true)
        {
            float displacement = Time.deltaTime * rotationPerSecond;
            rotateTransform.localRotation = Quaternion.Euler(Vector3.forward * (rotateTransform.localRotation.eulerAngles.z - displacement));

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ShiftOpacity()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / opacityShiftTime;
            float opacity = Mathf.Lerp(0, targetOpacity, time);

            main.color = new Color(main.color.r, main.color.g, main.color.b, opacity / 5);
            orb1.color = new Color(orb1.color.r, orb1.color.g, orb1.color.b, opacity);
            orb2.color = new Color(orb2.color.r, orb2.color.g, orb2.color.b, opacity);
            orb3.color = new Color(orb3.color.r, orb3.color.g, orb3.color.b, opacity);
            orb4.color = new Color(orb4.color.r, orb4.color.g, orb4.color.b, opacity);

            yield return new WaitForEndOfFrame();
        }

        time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / opacityFadeTime;
            float opacity = Mathf.Lerp(targetOpacity, 0, time);

            main.color = new Color(main.color.r, main.color.g, main.color.b, opacity / 5);
            orb1.color = new Color(orb1.color.r, orb1.color.g, orb1.color.b, opacity);
            orb2.color = new Color(orb2.color.r, orb2.color.g, orb2.color.b, opacity);
            orb3.color = new Color(orb3.color.r, orb3.color.g, orb3.color.b, opacity);
            orb4.color = new Color(orb4.color.r, orb4.color.g, orb4.color.b, opacity);

            yield return new WaitForEndOfFrame();
        }

        ore.visualActive = false;
        Destroy(gameObject);
    }
}
