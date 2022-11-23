using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private BoxCollider2D foot;
    [SerializeField] private Transform pickaxeAxis;

    [SerializeField] private float moveSpeed;

    private float frostSlowRatio = 0.4f;
    private float frostThreshold = 60f;

    // In-game values
    private bool slowed;

    private float currentFrost;

    private int currentEmerald;
    private int currentGold;
    private int currentSilver;
    private int currentVibralite;
    private int currentMoldalium;
    private int currentElectrite;
    private int currentRuby;
    private int currentDiamond;

    public void Initialize()
    {

    }

    public void ResetCurrentValues()
    {
        currentDiamond = 0;
        currentElectrite = 0;
        currentRuby = 0;
        currentMoldalium = 0;
        currentVibralite = 0;
        currentSilver = 0;
        currentGold = 0;
        currentEmerald = 0;

        currentFrost = 0;

        slowed = false;
    }

    void Update()
    {
        
    }

    public void AddFrost(float value)
    {
        currentFrost += value;

        if(currentFrost > frostThreshold)
        {
            slowed = true;
        }
        else
        {
            slowed = false;
        }
    }


}
