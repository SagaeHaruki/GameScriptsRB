using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IKSystem))]
[RequireComponent(typeof(AnimationScript))]
[RequireComponent(typeof(GroundedState))]
[RequireComponent(typeof(ChangeSpeed))]
[RequireComponent(typeof(KeyPressCheck))]
public class PlayerMovementV2 : MonoBehaviour
{
    #region
    // Camera Components
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] public Transform mainCamera;

    // Player Componets
    [SerializeField] public CharacterController charControl;
    [SerializeField] public Rigidbody playerRigid;
    [SerializeField] public Animator animator;
    [SerializeField] public LayerMask layerMasks;
    #endregion
    #region Player values
    [SerializeField] public float groundDistance = 0.1f;
    [SerializeField] public float playerSpeed = 3f;
    [SerializeField] public float speedModifier;
    [SerializeField] public float jumpForce = 4.2f;
    [SerializeField] public float jumpForwardForce = 2.3f;
    [SerializeField] public float FallingHeightDiff = 1.5f;
    [SerializeField] public float GlidingHeightDiff = 0.5f;
    #endregion

    #region Camera Movement Smoothness
    [SerializeField] private float turnSmoothing = 0.1f;
    private float smoothingVelocity;
    #endregion

    #region
    [SerializeField] public string playerState;
    [SerializeField] public bool isMoving;
    // Sorts of Movements
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isRunning;
    [SerializeField] public bool isSprinting;
    [SerializeField] public bool isJumping;
    [SerializeField] public bool isClimbing;
    [SerializeField] public bool isGliding;
    [SerializeField] public bool isAttacking;
    #endregion

    #region
    // Falling or Grounded
    [SerializeField] public bool isFalling;
    [SerializeField] public bool isGrounded;
    [SerializeField] public bool onSlope;
    [SerializeField] public bool goingUp;
    [SerializeField] public bool goingDown;
    // Gliding
    [SerializeField] public float glideSpeed = 5f;
    [SerializeField] public float maxGlideSpeed = 10f;
    [SerializeField] public float glideDownForceWhileGliding = 3f;
    #endregion


    private void Awake()
    {
        charControl = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
        isRunning = true;

        // Hides & lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float horizontal = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float vertical = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (isGrounded || isGliding)
        {
            if (direction.magnitude >= 0.1f && !isClimbing)
            {
                // This Section will calculate the direction of the player, then smoothens it rotation based on the calulated direction
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothingVelocity, turnSmoothing);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                float newSpeed = playerSpeed * speedModifier;

                Vector3 newDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                charControl.Move(newDirection * newSpeed * Time.deltaTime);
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
