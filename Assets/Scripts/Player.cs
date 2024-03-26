using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D body;
    Camera cam;

    bool inSmoke = false;
    bool enterSmoke;
    int smokeInt;

    [SerializeField] bool isMirrorSide;
    int mirrorInt;

    [Header("Ground Check")]
    bool isGround;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckOffset = new Vector2(0,-.5f);
    [SerializeField] float groundCheckLenght = .2f;

    [Header("Movement")]
    float inputX;
    [SerializeField] float walkSpeed = 1.5f;

    [Header("Jumping")]
    //jump
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float coyoteTime = .2f;
    [SerializeField] float bufferTime = .2f;

    float cTimer, bTimer;

    [Header("Smoke")]
    //smoke
    [SerializeField] float smokeThreshold = 1f;
    [SerializeField] float smokeRemoveRate = .1f;
    float currentSmokeLevel;

    [SerializeField] GameObject ConfuseUI;
    [SerializeField] Image ConfuseImageFill;
    [SerializeField] Vector2 uiOffset;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }
    
    public void EnterSmoke(bool value)
    {
        enterSmoke = value;
    }

    public void AddSmoke(float value)
    {
        currentSmokeLevel += value * Time.deltaTime;
    }

    void SmokeState()
    {
        if (currentSmokeLevel >= smokeThreshold)
        { 
            inSmoke = true;
            currentSmokeLevel = smokeThreshold;
        }

        if (currentSmokeLevel <= 0)
        {
            currentSmokeLevel = 0;
            inSmoke = false;
        }
        else if (!enterSmoke)
            currentSmokeLevel -= smokeRemoveRate * Time.deltaTime;

        if (ConfuseUI)
        {
            ConfuseUI.SetActive((currentSmokeLevel > 0));

            var newPos = cam.WorldToScreenPoint((Vector2)transform.position + uiOffset);

            ConfuseUI.transform.position = Vector2.Lerp(ConfuseUI.transform.position, newPos, .1f);
            ConfuseImageFill.fillAmount = currentSmokeLevel / smokeThreshold;
        }
    }


    // Update is called once per frame
    void Update()
    {
        mirrorInt = (isMirrorSide ? -1 : 1);
        smokeInt = (inSmoke ? -1 : 1);

        SmokeState();

        GroundCheck();

        GetInput();

        Movement();
        
        Jump();
    }

    void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");

    }

    void Movement()
    {
        body.velocity = new Vector2((inputX * walkSpeed) * mirrorInt * smokeInt, body.velocity.y);
    }

    void GroundCheck()
    {
        isGround = Physics2D.Raycast((Vector2)transform.position + groundCheckOffset, Vector2.down, groundCheckLenght, groundLayer);
    }

    void Jump()
    {
        //Timers

        if (isGround)
            cTimer = coyoteTime;
        else
            cTimer -= Time.deltaTime;

        //Imput

        if (Input.GetButtonDown("Jump"))
        {
            bTimer = bufferTime;
        }
        else
        {
            bTimer -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
        {
            cTimer = 0f;
        }

        //execution

        if (cTimer > 0 && bTimer > 0)
        {
            var jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * Mathf.Abs(body.gravityScale)));

            body.velocity = new Vector2(body.velocity.x, jumpForce);


            bTimer = 0f;
        }
    }

}
