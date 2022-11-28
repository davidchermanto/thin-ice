using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D foot;
    [SerializeField] private Transform pickaxeAxis;
    [SerializeField] private GameObject spriteObject;

    [SerializeField] private float moveSpeed;

    // Constants

    private float frostSlowRatio = 0.4f;
    private float frostThreshold = 60f;

    private float speed = 2.5f;

    // In-game values
    public bool isPlaying;

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
        StartCoroutine(Breathe());
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
            transform.position = new Vector3(transform.position.x - speed * Time.fixedDeltaTime, transform.position.y);   
        }
        else if(direction == "right")
        {
            transform.position = new Vector3(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y);
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

    private IEnumerator Breathe()
    {
        float yDown = -0.07f;
        float yDownTime = 4;
        float yUpTime = 3f;

        float time = 0;
        bool ping = false;
        Vector3 startPos = new Vector3(0, 0, 0);
        Vector3 endPos = new Vector3(startPos.x, yDown, 0);

        while (true)
        {
            if (!ping)
            {
                time += Time.deltaTime / yDownTime;

                spriteObject.transform.localPosition = Vector3.Lerp(startPos, endPos, time);

                if(time >= 1)
                {
                    ping = true;
                    time -= 1;
                }
            }
            else
            {
                time += Time.deltaTime / yUpTime;

                spriteObject.transform.localPosition = Vector3.Lerp(endPos, startPos, time);

                if (time >= 1)
                {
                    ping = false;
                    time -= 1;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
