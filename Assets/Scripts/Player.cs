using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D foot;
    [SerializeField] private Transform pickaxeAxis;

    [SerializeField] private float moveSpeed;

    // Constants

    private float frostSlowRatio = 0.4f;
    private float frostThreshold = 60f;

    private float speed = 20f;
    private float maxSpeed = 40f;

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
            transform.position = new Vector3(transform.position.x - 1f * Time.fixedDeltaTime, transform.position.y);   
        }
        else if(direction == "right")
        {
            transform.position = new Vector3(transform.position.x + 1f * Time.fixedDeltaTime, transform.position.y);
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
