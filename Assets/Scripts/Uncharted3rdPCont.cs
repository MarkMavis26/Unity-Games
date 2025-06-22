using System.Collections;
using UnityEngine;

public class Uncharted3rdPCont : MonoBehaviour
{
    //Character Movement
    public float movementSpeed = 7;
    public Rigidbody rb;
    public Animator animator;
    public GameObject mainCamera;
    public Vector3 moveDirection;
    //Character Rolling
    public float rollDistance = 10f;
    public float rollSpeed = 17f;
    public bool isRolling;

    //jump
    public float playerHeight;
    public GameObject groundCheckPoint;
    public float groundCheckDistance = 0.5f;
    public LayerMask whatIsGround;
    public float jumpForce = 7f;
    public float jumpCooldown = 0.25f;

    public bool grounded;
    public bool readyToJump = true;

    //aiming
    public GameObject combatCamera;
    public GameObject reticle;
    public AudioClip aimingSFX;
    public AudioSource audioSource;

    private float horizontalInput;
    private float verticalInput;
    bool isAiming;

    

    void Start()
    {
        
    }

    void Update()
    {
        // ground check
        float distanceToGround = playerHeight * groundCheckDistance;

        // Perform the raycast
        grounded = Physics.Raycast(groundCheckPoint.transform.position, Vector3.down, distanceToGround, whatIsGround);

        animator.SetBool("isGrounded", grounded);

       Movement();

       Roll();

       Jump();

       StartAiming();

       Shoot();

    }

    void Movement() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;
        Debug.Log(moveDirection);
        rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);

        //Animation
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

        float velocity = flatVel.magnitude;

        animator.SetFloat("Velocity", velocity);
    }
    void Roll()
    {
        if(Input.GetKeyDown(KeyCode.F) && isRolling == false)
        {
            //roll player
            animator.SetTrigger("Roll");

            Vector3 rollDirection;

            if (horizontalInput == 0)
            {
                Vector3 cameraForward = mainCamera.transform.forward;
                
                cameraForward.y = 0;

                cameraForward.Normalize();

                rollDirection = cameraForward;

                transform.forward = rollDirection;
            }
            else
            {
                rollDirection = transform.forward;
            }

            StartCoroutine(PerformRoll(rollDirection));
        }
    }

    private IEnumerator PerformRoll(Vector3 rollDirection)
    {
        isRolling = true;

        float startTime = Time.time;

        while (Time.time < startTime + rollDistance / rollSpeed)
        {
            rb.MovePosition(rb.position + rollDirection * rollSpeed * Time.deltaTime); 
            yield return null;
        }

        isRolling = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            // Trigger jump start animation
            animator.SetTrigger("Start Jump");

            // reset y velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    void StartAiming()
    {
        if(Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isAiming", true); 

            combatCamera.SetActive(true);
            reticle.SetActive(true);

            isAiming = true;

            audioSource.clip = aimingSFX;
            audioSource.Play();

            //activate aim camera rotations
            AimCamera aimCameraScript = combatCamera.GetComponent<AimCamera>();
            aimCameraScript.StartAiming();
        }

        if(Input.GetMouseButtonUp(1))
        {
            animator.SetBool("isAiming", false);

            combatCamera.SetActive(false);
            reticle.SetActive(false);

            isAiming = false;

            //stop aiming
            AimCamera aimCameraScript = combatCamera.GetComponent<AimCamera>();
            aimCameraScript.StopAiming();
        }
    }

    void Shoot()
    {
        if(Input.GetMouseButtonDown(0) && isAiming)
        {
            GetComponent<ShootingSystem>().Shoot();
            animator.SetTrigger("Shoot");
        }
    }


}
