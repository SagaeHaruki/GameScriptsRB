using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IKSystem))]
public class PlayerMovement : MonoBehaviour
{
    #region
    [SerializeField] private Rigidbody playerControl;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private LayerMask layerMasks;
    #endregion

    #region Player values
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float speedModifier = 1f;
    [SerializeField] private float jumpForce = 4.2f;
    #endregion

    #region Camera Movement Smoothness
    [SerializeField] private float turnSmoothing = 0.1f;
    private float smoothingVelocity;
    #endregion

    [SerializeField] public string playerState;
    [SerializeField] public bool isMoving;
    // Sorts of Movements
    [SerializeField] public bool isJumping;
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isRunning;
    [SerializeField] public bool isSprinting;
    [SerializeField] private bool isAttacking;
    [SerializeField] public bool currentlyMoving;

    // Falling or Grounded
    [SerializeField] public bool isFalling;
    [SerializeField] public bool isGrounded;
    [SerializeField] public bool onSlope;
    [SerializeField] private bool goingUp;
    [SerializeField] private bool goingDown;

    private void Awake()
    {
        playerControl = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Hides & lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        showCursor();
        velLimit();
        GetKeyPress();

        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);

        isGrounded = Physics.CapsuleCast(point1, point2, capsuleCollider.radius * 0.9f, Vector3.down, 0.1f);

        float horizontal = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float vertical = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // This Section will calculate the direction of the player, then smoothens it rotation based on the calulated direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothingVelocity, turnSmoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 newDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerControl.AddForce(newDirection.normalized * playerSpeed * speedModifier, ForceMode.Force);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void GetKeyPress()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                isJumping = true;
                playerControl.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (isGrounded)
            {
                isJumping = false;
            }
        }
    }

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
}
