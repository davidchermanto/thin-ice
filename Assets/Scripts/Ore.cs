using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Ore : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private string oreName;
    [SerializeField] private Sprite oreSprite;
    [SerializeField] private Color color;

    // Vibration / Combat
    [SerializeField] private int hitsToDestroy;
    [SerializeField] private float damageToIce;
    // Value = 1.5 + actual radius
    [SerializeField] private float damageRadius;

    // Pre-assigned
    [SerializeField] private CircleCollider2D oreCollider2D;
    [SerializeField] private SpriteRenderer gemSprite;
    [SerializeField] private Light2D glow;
    [SerializeField] private GameObject hitEffect;

    //
    [SerializeField] private List<IceBlock> iceBlocksInRange;

    public void Initialize(string name, Sprite oreSprite, Color color, int hitsToDestroy, float damageToIce, 
        float damageRadius, GameManager gameManager)
    {
        this.oreName = name;
        this.oreSprite = oreSprite;
        this.hitsToDestroy = hitsToDestroy;
        this.damageToIce = damageToIce;
        this.damageRadius = damageRadius;

        this.gameManager = gameManager;

        oreCollider2D.radius = damageRadius + 1.5f;

        gemSprite.sprite = oreSprite;
        gemSprite.color = color;
        glow.color = color;
    }

    public void HitOre()
    {
        VibrateDamage();
        hitsToDestroy -= 1;

        if(hitsToDestroy <= 0)
        {
            Die();
        }
    }

    public void VibrateDamage()
    {
        
    }

    private void Die()
    {

    }

    public void AddIce(IceBlock ice)
    {
        iceBlocksInRange.Add(ice);
    }
}
