using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D foot;
    [SerializeField] private Transform pickaxeAxis;
    [SerializeField] private GameObject spriteObject;

    [SerializeField] private Transform cart;
    [SerializeField] private Transform leftWheel;
    [SerializeField] private Transform rightWheel;

    // Constants

    private float frostSlowRatio = 0.4f;
    private float frostThreshold = 60f;

    private float speed = 2.5f;

    private float rotateSpeed = 340f;
    [SerializeField] private bool moving = false;
    [SerializeField] private bool goingRight = true;

    private float maxCartDisplacement = 0.04f;
    private float maxDisplacementPerSecond = 0.36f;

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
        StartCoroutine(WheelRotate());
        isPlaying = true;
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
        if (isPlaying)
        {
            if (Input.GetKey(KeyCode.D))
            {
                MoveCharacter("right");
                moving = true;
                goingRight = true;
            }
            else
            {
                moving = false;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                MoveCharacter("left");
            }

        }
    }

    private void MoveCharacter(string direction)
    {
        if(direction == "right")
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
        float yDown = -0.1f;
        float yDownTime = 4;
        float yUpTime = 4;

        float time = 0;
        bool ping = false;
        Vector3 startPos = new Vector3(spriteObject.transform.localPosition.x, spriteObject.transform.localPosition.y, 0);
        Vector3 endPos = new Vector3(startPos.x, startPos.y + yDown, 0);

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

    private IEnumerator WheelRotate()
    {
        Vector3 cartPosLimit = cart.transform.localPosition + new Vector3(maxCartDisplacement, maxCartDisplacement);

        while (true)
        {
            if (moving)
            {
                float displacement = Time.deltaTime * rotateSpeed;

                if (goingRight)
                {
                    rightWheel.localRotation = Quaternion.Euler(Vector3.forward * (rightWheel.localRotation.eulerAngles.z - displacement));
                    leftWheel.localRotation = Quaternion.Euler(Vector3.forward * (leftWheel.localRotation.eulerAngles.z - displacement));
                }
                else
                {
                    rightWheel.localRotation = Quaternion.Euler(Vector3.forward * (rightWheel.localRotation.eulerAngles.z + displacement));
                    leftWheel.localRotation = Quaternion.Euler(Vector3.forward * (leftWheel.localRotation.eulerAngles.z + displacement));
                }

                float displacementCart = Time.deltaTime * maxDisplacementPerSecond;
                Vector3 cartMovement;

                if(Random.Range(0, 2) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        cartMovement = new Vector3(displacementCart, 0);
                    }
                    else
                    {
                        cartMovement = new Vector3(-displacementCart, 0);
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        cartMovement = new Vector3(0, displacementCart);
                    }
                    else
                    {
                        cartMovement = new Vector3(0, -displacementCart);
                    }
                }

                cart.transform.localPosition += cartMovement;
                //cart.transform.localPosition = Vector3.ClampMagnitude(cartPosLimit, maxCartDisplacement);

                if(cart.transform.localPosition.x > cartPosLimit.x)
                {
                    cart.transform.localPosition = new Vector3(cartPosLimit.x, cart.transform.localPosition.y);
                }
                if (cart.transform.localPosition.x < -cartPosLimit.x)
                {
                    cart.transform.localPosition = new Vector3(-cartPosLimit.x, cart.transform.localPosition.y);
                }

                if (cart.transform.localPosition.y > cartPosLimit.y)
                {
                    cart.transform.localPosition = new Vector3(cart.transform.localPosition.x, cartPosLimit.y);
                }
                if (cart.transform.localPosition.y < -cartPosLimit.y)
                {
                    cart.transform.localPosition = new Vector3(cart.transform.localPosition.x, -cartPosLimit.y);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
