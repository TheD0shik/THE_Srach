using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runMultiplier = 1.5f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 7f;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    private float originalHeight;
    private Vector3 originalCenter;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Ground Check")]
    public LayerMask groundMask;
    public float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isCrouching;
    private bool isGrounded;
    private float currentSpeed;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;
        currentSpeed = walkSpeed;
        rb.freezeRotation = true;
    }

    void Update()
    {
        GroundCheck();
        HandleJump();
        HandleCrouch();
        HandleRun();
        anim();
    }
    void anim()
    {
        if (Input.GetKeyDown(KeyCode.W))
            animator.Play("walk");

    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    void GroundCheck()
    {
        float castDistance = capsuleCollider.height / 2 + groundCheckDistance;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, castDistance, groundMask);
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraForward * moveVertical + cameraRight * moveHorizontal).normalized;
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }
//&& isGrounded
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isCrouching) StopCrouch();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl)) StopCrouch();
    }

    void HandleRun()
    {
        currentSpeed = (Input.GetKey(KeyCode.LeftShift) && !isCrouching) ? 
            walkSpeed * runMultiplier : walkSpeed;
    }

    void StartCrouch()
    {
        isCrouching = true;
        capsuleCollider.height = crouchHeight;
        capsuleCollider.center = originalCenter * 0.5f;
        currentSpeed = crouchSpeed;
    }

    void StopCrouch()
    {
        if (!Physics.Raycast(transform.position, Vector3.up, originalHeight))
        {
            isCrouching = false;
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = originalCenter;
            currentSpeed = walkSpeed;
        }
    }
}