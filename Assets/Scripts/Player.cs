using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private BoxCollider2D foot;
    [SerializeField] private Transform pickaxeAxis;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;

    // Constants

    private float frostSlowRatio = 0.4f;
    private float frostThreshold = 60f;

    private float speed = 100f;
    private float maxSpeed = 80f;

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

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            MoveCharacter("right");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveCharacter("left");
        }
    }

    private void MoveCharacter(string direction)
    {
        if (direction == "left")
        {
            rb.AddForce(new Vector2(-speed * Time.deltaTime, 0));
        }
        else if(direction == "right")
        {
            rb.AddForce(new Vector2(speed * Time.deltaTime, 0));
        }
        
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
