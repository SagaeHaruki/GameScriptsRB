using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(IKSystem))]
[RequireComponent(typeof(AnimationScript))]
[RequireComponent(typeof(PlayerSpeedChange))]
[RequireComponent(typeof(ChangeState))]
public class PlayerMovement : MonoBehaviour
{
    #region Instances
    AnimationScript Animscript;
    PlayerSpeedChange speedChange;
    #endregion

    #region
    [SerializeField] public Rigidbody playerControl;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform mainCamera;
    [SerializeField] public LayerMask layerMasks;
    #endregion

    #region Player values
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] public float speedModifier;
    [SerializeField] private float jumpForce = 4.2f;
    [SerializeField] private float jumpForwardForce = 2.3f;
    [SerializeField] private float FallingHeightDiff = 2.2f;
    [SerializeField] private float GlidingHeightDiff = 1.8f;
    #endregion

    #region Camera Movement Smoothness
    [SerializeField] private float turnSmoothing = 0.1f;
    private float smoothingVelocity;
    #endregion

    #region
    [SerializeField] private string playerState;
    [SerializeField] public bool isMoving;
    // Sorts of Movements
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isRunning;
    [SerializeField] public bool isSprinting;
    [SerializeField] public bool isClimbing;
    [SerializeField] public bool isGliding;
    [SerializeField] public bool isAttacking;
    #endregion

    #region
    // Falling or Grounded
    [SerializeField] public bool isFalling;
    [SerializeField] public bool isGrounded;

    [SerializeField] public float glideSpeed = 5f;
    [SerializeField] public float maxGlideSpeed = 10f;
    [SerializeField] public float glideDownForceWhileGliding = 3f;
    #endregion

    private void Awake()
    {
        playerControl = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        speedChange = GetComponent<PlayerSpeedChange>();
        isRunning = true;

        // Hides & lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        showCursor();
        velLimit();
        GetKeyPress();
        ChangeState();
        FallingState();
        JumpHandlerSection();
        MovePlayerSection();

        speedModifier = speedChange.speedModifier;
    }

    private void MovePlayerSection()
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
                playerControl.AddForce(newDirection.normalized * newSpeed, ForceMode.Force);
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    private void ChangeState()
    {
        if (!isFalling)
        {
            if (isMoving)
            {
                if (isRunning)
                {
                    playerState = "Running";
                }

                if (isWalking)
                {
                    playerState = "Walking";
                }

                if (isSprinting)
                {
                    playerState = "Sprinting";
                }

                if (isGliding)
                {
                    playerState = "Gliding";
                }
            }
            else
            {
                playerState = "Idle";
            }
        }
        else
        {
            playerState = "Falling";
        }
    }


    #region Key Press Section
    private void GetKeyPress()
    {
        // Toggle between Walk and Sprint Section
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isRunning)
            {
                isRunning = false;
                isWalking = true;
            }
            else
            {
                isRunning = true;
                isWalking = false;
            }
        }
    }

    private void JumpHandlerSection()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Calculate the forward jump direction
                Vector3 forwardJumpDirection = transform.forward * jumpForwardForce;

                // Apply the forward jump force using AddForce
                playerControl.AddForce(forwardJumpDirection, ForceMode.VelocityChange);

                // Apply the upward jump force
                playerControl.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                if (isWalking)
                {

                }

                if (isRunning)
                {

                }

                if (isSprinting)
                {

                }
            }
        }

        if (!isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isGliding)
            {
                RaycastHit hit;

                Ray ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMasks))
                {
                    float currentHeight = hit.distance;

                    if (currentHeight >= FallingHeightDiff)
                    {
                        if (isFalling)
                        {
                            isFalling = false;   
                            isGliding = true;
                        }

                        if (!isFalling)
                        {
                            isGliding = true;
                        }
                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.Space) && isGliding)
            {
                isGliding = false;
            }
        }

        if (isGrounded && isGliding)
        {
            isGliding = false;
        }
    }
    #endregion

    #region
    public void FallingState()
    {
        // Section that checks if the player is grounded or not
        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        isGrounded = Physics.CapsuleCast(point1, point2, capsuleCollider.radius * 0.9f, Vector3.down, 0.1f);

        // If not grounded change the state to falling
        if (!isGrounded)
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMasks))
            {
                float currentHeight = hit.distance;

                if (currentHeight >= FallingHeightDiff)
                {
                    if (!isGliding)
                    {
                        isFalling = true;
                    }
                }
            }
        }
        else
        {
            isFalling = false;
        }


        if (isGliding)
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMasks))
            {
                float currentHeight = hit.distance;
                if (currentHeight >= GlidingHeightDiff)
                {
                    // Calculate the desired velocity while gliding
                    Vector3 desiredVelocity = transform.forward * glideSpeed;

                    // Limit the maximum speed while gliding
                    if (desiredVelocity.magnitude > maxGlideSpeed)
                    {
                        desiredVelocity = desiredVelocity.normalized * maxGlideSpeed;
                    }

                    // Set the Rigidbody's velocity directly
                    playerControl.velocity = desiredVelocity;

                    // Apply a different downward force while gliding
                    playerControl.AddForce(Vector3.down * glideDownForceWhileGliding, ForceMode.Acceleration);

                }
            }
        }
        else
        {
            playerControl.useGravity = true;
            playerControl.drag = 0f;
        }
    }
    #endregion

    #region Some usefull stuffs will happen here
    private void velLimit()
    {
        Vector3 flatVel = new Vector3(playerControl.velocity.x, 0f, playerControl.velocity.z);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector3 limitVel = flatVel.normalized * playerSpeed;
            playerControl.velocity = new Vector3(limitVel.x, playerControl.velocity.y, limitVel.z);
        }

    }

    private void showCursor()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion
}
