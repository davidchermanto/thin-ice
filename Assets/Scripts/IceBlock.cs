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
    private float durabilityPerSteppedSecond = 0.33f;

    public void InitIce(int id, float durability, GameManager gameManager)
    {
        this.id = id;
        this.durability = durability;
        this.gameManager = gameManager;

        SetRandomColor();
        StartCoroutine(DurabilityTest());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            steppedOn = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            steppedOn = false;
        }
    }

    private void SetRandomColor()
    {
        float a = Random.Range(0.5f, 0.9f);
        float r = Random.Range(0.4f, 0.7f);
        float g = Random.Range(0.7f, 0.8f);
        float b = Random.Range(0.9f, 1f);

        color = new Color(r, g, b, a);
        spriteRenderer.color = color;
    }

    public void Die()
    {
        gameManager.RequestIceDie(this);
        StartCoroutine(AnimateFall());
    }

    public void BlinkDurability()
    {

    }

    public IEnumerator Animate()
    {
        float maxUp = 0.15f;
        bool rising = false;

        while (gameObject.activeSelf)
        {

            yield return new WaitForFixedUpdate();
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
            }

            yield return new WaitForFixedUpdate();
        }

        Die();
    }

    private IEnumerator AnimateFall()
    {
        yield return new WaitForFixedUpdate();

    }
}
