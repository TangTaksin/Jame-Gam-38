using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D body;
    public Camera cam;

    bool inSmoke = false;
    bool enterSmoke;
    int smokeInt;

    private Animator animator;

    [SerializeField] bool isMirrorSide;
    int mirrorInt;

    [Header("Ground Check")]
    bool isGround;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckOffset = new Vector2(0, -.5f);
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

    public delegate void DieEvent();
    public static DieEvent OnDie;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        LevelManager.OnPause += Pause;
        LevelManager.OnUnPause += UnPause;
    }

    private void OnDestroy()
    {
        LevelManager.OnPause -= Pause;
        LevelManager.OnUnPause -= UnPause;
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
            animator.SetBool("IsConfused", !inSmoke && inputX == 0);
            AudioManager.Instance.minTimeBetweenFootsteps = .1f;
            AudioManager.Instance.maxTimeBetweenFootsteps = .15f;
        }

        if (currentSmokeLevel <= 0)
        {
            currentSmokeLevel = 0;
            inSmoke = false;
            Debug.Log("Setting IsConfused to false");
            animator.SetBool("IsConfused", inSmoke);
            AudioManager.Instance.minTimeBetweenFootsteps = .3f;
            AudioManager.Instance.maxTimeBetweenFootsteps = .42f;
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
        if (isPause)
            return;

        mirrorInt = (isMirrorSide ? -1 : 1);
        smokeInt = (inSmoke ? -1 : 1);

        SmokeState();
        GroundCheck();
        GetInput();
        Movement();
        PlayWalkSFX();
        Jump();

    }

    private void PlayWalkSFX()
    {
        if (inputX != 0 && isMirrorSide != true && isGround)
        {
            AudioManager.Instance.PlayWalkSFX();
        }
    }

    void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
    }

    void Movement()
    {
        if (inSmoke && inputX != 0)
        {
            animator.SetBool("IsWalkingConfused", true);

        }
        else
        {
            animator.SetBool("IsWalkingConfused", false);
        }

        if (inSmoke && inputX == 0)
        {
            animator.SetBool("IsConfused", true);

        }
        else
        {
            animator.SetBool("IsConfused", false);
        }

        FlipScalePlayer();


        body.velocity = new Vector2((inputX * walkSpeed) * mirrorInt * smokeInt, body.velocity.y);

        if (inputX != 0 && isGround)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    void FlipScalePlayer()
    {
        if (inputX < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inputX > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (inSmoke && inputX < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inSmoke && inputX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }

    }

    void GroundCheck()
    {
        isGround = Physics2D.Raycast((Vector2)transform.position + groundCheckOffset, Vector2.down, groundCheckLenght, groundLayer);
        animator.SetBool("Jumping", !isGround);
        if (isGround && cTimer != coyoteTime)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.footstepSounds[1]);
        }
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
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            cTimer = 0f;
        }

        //execution

        if (cTimer > 0 && bTimer > 0)
        {
            var jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * Mathf.Abs(body.gravityScale)));

            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetBool("Jumping", true);
            if (isMirrorSide == false)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSfx);

            }
            else
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSfx2);

            }



            bTimer = 0f;
        }
    }


    public void Die()
    {
        //play animation


        OnDie?.Invoke();
    }


    Vector2 saveVelo;
    bool isPause = false;

    void Pause()
    {
        isPause = true;
        body.isKinematic = true;
        saveVelo = body.velocity;
        body.velocity = Vector2.zero;
    }

    void UnPause()
    {
        isPause = false;
        body.isKinematic = false;
        body.velocity = saveVelo;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Draw a line to represent the ground check length
        Vector3 start = transform.position + new Vector3(groundCheckOffset.x, groundCheckOffset.y, 0);
        Vector3 end = start + Vector3.down * groundCheckLenght;
        Gizmos.DrawLine(start, end);
    }

}
