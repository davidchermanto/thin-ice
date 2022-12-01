using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [SerializeField] public BoxCollider2D iceCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject explodeParticles;

    // 0 to 100. When reaches 0, ice dies
    [SerializeField] private float durability;

    // What number of ice is this
    [SerializeField] private int id;

    private Color color;
    [SerializeField] private bool steppedOn;

    private GameManager gameManager;

    // Constants
    private float durabilityWhenSteppedOn = 0.5f;
    private float durabilityPerSteppedSecond = 0.5f;

    private float durabilityBeforeAlphaChange = 10;
    private float maxAlpha = 0.45f;
    private float minAlpha = 0.2f;

    public void InitIce(int id, float durability, GameManager gameManager)
    {
        this.id = id;
        this.durability = durability;
        this.gameManager = gameManager;

        SetRandomColor();
        StartCoroutine(DurabilityTest());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            steppedOn = true;
            durability -= durabilityWhenSteppedOn;
            gameManager.UpdateFurthestActiveIce(id);
            StartCoroutine(Down());
        }
        else if (collision.gameObject.CompareTag("OreMain"))
        {
            collision.gameObject.GetComponent<Ore>().AddIce(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            steppedOn = false;
            StartCoroutine(Up());
        }
    }

    private IEnumerator Down()
    {
        float downTarget = -4.3f;
        float downTime = 0.3f;
        StopCoroutine(Up());

        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(currentPos.x, downTarget, 0);

        float time = 0;

        while(time < 1 && steppedOn)
        {
            time += Time.deltaTime / downTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, time);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Up()
    {
        float upTarget = -4.2f;
        float upTime = 0.8f;
        StopCoroutine(Down());

        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(currentPos.x, upTarget, 0);

        float time = 0;

        while (time < 1 && !steppedOn)
        {
            time += Time.deltaTime / upTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, time);

            yield return new WaitForEndOfFrame();
        }
    }

    private void SetRandomColor()
    {
        float a = durability / 100;
        float r = Random.Range(0.2f, 0.3f);
        float g = Random.Range(0.2f, 0.3f);
        float b = Random.Range(0.4f, 0.5f);

        color = new Color(r, g, b, a);
        spriteRenderer.color = color;

        UpdateAlpha();
    }

    public void Die()
    {
        gameManager.Die(this);
    }

    public void BlinkDurability()
    {

    }

    public void Explode()
    {
        GameObject effect = Instantiate(explodeParticles);
        effect.transform.position = transform.position;
        gameObject.SetActive(false);
    }

    public void TakeVibrationDamage(float damage)
    {
        durability -= damage;

        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        if(durability > durabilityBeforeAlphaChange)
        {
            spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Lerp(maxAlpha, 1, Mathf.Min(durability * 1.4f / 100, 1)));
        }
        else
        {
            float a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.Max(durability * 1.4f / 100, 0));
            spriteRenderer.color = new Color(color.r, color.g, color.b, a);
        }
    }

    private IEnumerator DurabilityTest()
    {
        float elapsed;

        while (true)
        {
            if (steppedOn)
            {
                elapsed = Time.deltaTime * durabilityPerSteppedSecond;
                durability -= elapsed;

                if(durability <= 0 && steppedOn)
                {
                    Die();
                }

                UpdateAlpha();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
