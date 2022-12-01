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

    private float baseSpeed = 1f;
    private float speed = 2.5f;

    private float rotateSpeed = 340f;
    [SerializeField] public bool moving = false;
    [SerializeField] public bool goingRight = true;

    private float maxCartDisplacement = 0.02f;
    private float maxDisplacementPerSecond = 0.36f;

    // In-game values
    public bool isPlaying;

    public bool accRight;
    public bool accLeft;

    public void Initialize()
    {
        StartCoroutine(Breathe());
        StartCoroutine(WheelRotate());
        isPlaying = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.D) && isPlaying)
        {
            moving = true;
            goingRight = true;
        }

        if (isPlaying && moving)
        {
            if (Input.GetKey(KeyCode.D) || accRight)
            {
                MoveCharacter("right");
            }

            transform.position = new Vector3(transform.position.x + baseSpeed * Time.fixedDeltaTime, transform.position.y);
        }
    }

    private void MoveCharacter(string direction)
    {
        if(direction == "right")
        {
            transform.position = new Vector3(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y);
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

    public IEnumerator WheelRotate()
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
