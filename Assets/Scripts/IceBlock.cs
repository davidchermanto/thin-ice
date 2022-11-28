using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D iceCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // 0 to 100. When reaches 0, ice dies
    [SerializeField] private float durability;

    // What number of ice is this
    [SerializeField] private int id;

    private Color color;
    private bool steppedOn;

    private GameManager gameManager;

    // Constants
    private float durabilityWhenSteppedOn = 2f;
    private float durabilityPerSteppedSecond = 0.66f;

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

        while(time < 1)
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

        while (time < 1)
        {
            time += Time.deltaTime / upTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, time);

            yield return new WaitForEndOfFrame();
        }
    }

    private void SetRandomColor()
    {
        float a = durability / 100;
        float r = Random.Range(0.5f, 0.6f);
        float g = Random.Range(0.7f, 0.8f);
        float b = Random.Range(0.9f, 1f);

        color = new Color(r, g, b, a);
        spriteRenderer.color = color;

        UpdateAlpha();
    }

    public void Die()
    {
        gameManager.RequestIceDie(this);
        StartCoroutine(AnimateFall());
    }

    public void BlinkDurability()
    {

    }

    public void TakeVibrationDamage(float damage)
    {
        durability -= damage;

        if (durability <= 0)
        {
            Die();
        }

        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        if(durability > 30)
        {
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.9f);
        }
        else
        {
            float a = (durability / 100) * 0.9f;
            spriteRenderer.color = new Color(color.r, color.g, color.b, a);
        }
    }

    private IEnumerator DurabilityTest()
    {
        float elapsed;

        while (durability > 0)
        {
            if (steppedOn)
            {
                elapsed = Time.deltaTime * durabilityPerSteppedSecond;
                durability -= elapsed;

                if(durability <= 0)
                {
                    Die();
                }

                UpdateAlpha();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator AnimateFall()
    {
        yield return new WaitForFixedUpdate();

    }
}
