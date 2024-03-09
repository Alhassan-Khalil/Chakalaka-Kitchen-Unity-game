using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [Header("abilty")]
    public bool canMove = true;
    public bool canSprint = true;
    public bool canJump = true;
    public bool canCrouch = true;
    public bool canZoom = true;

    [Header("Custom Flags for Movement Options")]
    public bool crouchAsToggle = true;

    private bool isZoomed;
    public bool isWalking { get; private set; }
    private bool isSprinting;
    private bool isJumping;
    private bool isCrouching;

    [Space(10)]
    [Header("Movement Paramenters")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float speedChangeRate = 10.0f;
    // Crouching Stuff //
    private float standingHeight = 0.0f;
    private float crouchingHeight = 0.0f;
    private float previousCharacterHeight = 0.0f;
    //The denominator with which we divide "standing" height to determine crouching height
    private float characterCrouchDivisor = 2.0f;

    private Vector3 cameraOffsetStanding = Vector3.zero;
    private Vector3 cameraOffsetCrouching = Vector3.zero;

    public float RotationSpeed = 1.0f;

    [Space(10)]
    [SerializeField] private float JumpHeight = 1.2f;
    [SerializeField] private float Gravity = -30f;

    [Space(10)]
    [SerializeField] private float JumpTimeout = 0.1f;
    [SerializeField] private float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [SerializeField] private bool Grounded = true;
    [SerializeField] private float GroundedOffset = -0.14f;
    [SerializeField] private float GroundedRadius = 0.5f;
    [SerializeField] private LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject CameraRoot;
    [SerializeField] private CinemachineVirtualCamera PlayerCamera;

    [SerializeField] private float fov = 60f;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float zoomStepTime = 5f;

    [SerializeField] private bool playerFrozen = false;

    private float speed;
    private float verticalVelocity;
    private float _rotationVelocity;
    private float terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    // animation IDs
    private float animationBlend;

    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;

    private PlayerInputManager inputHander;
    private CharacterController Controller;
    private Animator animator;
    private bool hasAnimator;


    private CollisionFlags CollisionFlags;

    private GameObject MainCameraRoot;


    private const float threshold = 0.01f;


    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        inputHander = GetComponent<PlayerInputManager>();
        hasAnimator = TryGetComponent(out animator);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        standingHeight = Controller.height;
        crouchingHeight = standingHeight / characterCrouchDivisor;
        previousCharacterHeight = standingHeight;

        cameraOffsetStanding = CameraRoot.transform.localPosition;
        cameraOffsetCrouching = cameraOffsetStanding;
        cameraOffsetCrouching.y -= 0.6f;

        // get a reference to our main camera
        if (MainCameraRoot == null)
        {
            MainCameraRoot = GameObject.FindGameObjectWithTag("MainCamera");
        }

    }
    private void Start()
    {       // reset our timeouts on start
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;

        isJumping = false;

        gameObject.GetComponent<MouseLook>().Init(transform, CameraRoot.transform);
        AssignAnimationIDs();

    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    // Update is called once per frame
    private void Update()
    {
        //hasAnimator = TryGetComponent(out animator);

        CheckGrounded();

        if (canJump && !isJumping && !isCrouching) Jump();

        if (canCrouch && inputHander.Crouch)
        {
            Crouch();
        }
        else
        {
            isCrouching = false;
        }

        if (canZoom)
        {
            if (inputHander.Zoom && !isSprinting)
            {
                isZoomed = true;
            }
            else
            {
                isZoomed = false;
            }

            if (isZoomed)
            {
                PlayerCamera.m_Lens.FieldOfView = Mathf.Lerp(PlayerCamera.m_Lens.FieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if (!isZoomed && !isSprinting)
            {
                PlayerCamera.m_Lens.FieldOfView = Mathf.Lerp(PlayerCamera.m_Lens.FieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
        }
    }
    private void FixedUpdate()
    {
        CameraRotation();
        if (canMove)
        {
            Move();
        }
    }
    private void CameraRotation()
    {
        gameObject.GetComponent<MouseLook>().LookRotation(transform, CameraRoot.transform);
    }

    private void Move()
    {

        if (isCrouching)
        {
            gameObject.GetComponent<CharacterController>().height = Mathf.Lerp(Controller.height, crouchingHeight, 7 * Time.fixedDeltaTime);
        }
        else
        {
            gameObject.GetComponent<CharacterController>().height = Mathf.Lerp(Controller.height, standingHeight, 7 * Time.fixedDeltaTime);
        }


        // We move the transform to be the x/z and exactly middle of Y relative to controller height change from crouch/stand
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (Controller.height - previousCharacterHeight) / 2, gameObject.transform.position.z);

        float targetSpeed = 0.0f;
        GetInput(out targetSpeed);


        if (inputHander.Move == Vector2.zero) targetSpeed = 0.0f;
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(Controller.velocity.x, 0.0f, Controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = inputHander.Move.magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(inputHander.Move.x, 0.0f, inputHander.Move.y).normalized;
        // if there is a move input rotate player when the player is moving
        if (inputHander.Move != Vector2.zero)
        {
            // move
            inputDirection = transform.right * inputHander.Move.x + transform.forward * inputHander.Move.y;
        }
        CollisionFlags = Controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        // update animator if using character
        if (hasAnimator)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }

        UpdateCameraPosition();
    }
    private void UpdateCameraPosition()
    {
        if (isCrouching)
        {
            CameraRoot.transform.localPosition = Vector3.Lerp(CameraRoot.transform.localPosition, cameraOffsetCrouching, 0.1f);
        }
        else
        {
            CameraRoot.transform.localPosition = Vector3.Lerp(CameraRoot.transform.localPosition, cameraOffsetStanding, 0.1f);
        }
    }
    private void Crouch()
    {
        if (canMove && Controller.isGrounded)
        {
            if (crouchAsToggle)
            {
                if (inputHander.Crouch)
                {
                    if (isCrouching)
                    {
                        if (haveHeadRoomToStand())
                        {
                            isCrouching = false;
                        }
                    }
                    else
                    {
                        isCrouching = true;
                    }
                }
            }
            else
            {
                if (inputHander.Crouch)
                {
                    isCrouching = true;
                }
                else
                {
                    if (isCrouching)
                    {
                        if (haveHeadRoomToStand())
                        {
                            isCrouching = false;
                        }
                    }
                }
            }
        }
        previousCharacterHeight = Controller.height;
    }


    private void GetInput(out float speed)
    {
        isWalking = (canMove && (inputHander.Move != Vector2.zero));
        isSprinting = (canSprint && inputHander.Sprint);

        if (isCrouching || (canZoom && inputHander.Zoom))
        {
            speed = crouchSpeed;
        }
        else
        {
            speed = canSprint && inputHander.Sprint ? sprintSpeed : moveSpeed;
        }

    }
    private bool haveHeadRoomToStand()
    {

        bool haveHeadRoom = true;
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.up * (standingHeight - (Controller.height/2.0f)), Color.red);

        RaycastHit headRoomHit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.up, out headRoomHit, (standingHeight - (Controller.height / 2.0f))))
        {
            //Debug.Log("Headroom hit " + headRoomHit.collider.transform.name);
            haveHeadRoom = false;
        }

        return haveHeadRoom;

    }


    private void Jump()
    {
        if (Grounded)
        {
            fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (inputHander.Jump && jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, true);
                }
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = JumpTimeout;
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(animIDFreeFall, true);
                }
            }
            //inputHander.Jump = false;
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }

    }

    private void CheckGrounded()
    {
        //TODO: change this to better one 
        //Grounded = Controller.isGrounded;
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (hasAnimator)
        {
            animator.SetBool(animIDGrounded, Grounded);
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Rigidbody body = hit.collider.attachedRigidbody;

        // Don't move the rigidbody if the character is on top of it
        if (CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }

        body.AddForceAtPosition(Controller.velocity * 0.1f, hit.point, ForceMode.Impulse);

    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    public void enableMovement()
    {
        canMove = true;
    }

    public void disableMovement()
    {
        canMove = false;
    }

    public void enableRun()
    {
        canSprint = true;
    }

    public void disableRun()
    {
        canSprint = false;
    }

    public void enableJump()
    {
        canJump = true;
    }

    public void disableJump()
    {
        canJump = false;
    }

    public void enableCrouch()
    {
        canCrouch = true;
    }

    public void disableCrouch()
    {
        canCrouch = false;
    }
}
