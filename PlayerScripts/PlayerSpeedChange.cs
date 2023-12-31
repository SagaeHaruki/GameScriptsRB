using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedChange : MonoBehaviour
{
    PlayerMovement instance;

    #region
    [SerializeField] public float speedModifier;
    #endregion


    #region
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isGliding;
    [SerializeField] private bool isMoving;


    [SerializeField] private bool onSlope;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool goingDown;
    [SerializeField] private bool goingUp;
    #endregion

    #region
    private float slopeAngle;
    private float maxRayDistance = 1.0f;
    private LayerMask LayerMasks;
    #endregion

    private float previousYPosition;

    private void Awake()
    {
        instance = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        GetSlope();
        AdjustMoveSpeed();

        LayerMasks = instance.layerMasks;
        isMoving = instance.isMoving;
        isGrounded = instance.isGrounded;
        isWalking = instance.isWalking;
        isRunning = instance.isRunning;
        isSprinting = instance.isSprinting;
        isGliding = instance.isGliding;
    }

    private void GetSlope()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, maxRayDistance, LayerMasks))
        {
            Vector3 groundNormal = hit.normal;
            slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
            float currentYPosition = transform.position.y;
            if (slopeAngle >= 1)
            {
                onSlope = true;
                if (isMoving)
                {
                    if (currentYPosition > previousYPosition)
                    {
                        goingUp = true;
                        goingDown = false;
                    }
                    else if (currentYPosition < previousYPosition)
                    {
                        goingUp = false;
                        goingDown = true;
                    }
                    else
                    {
                        goingDown = false;
                        goingUp = false;
                        return;
                    }
                }
                else
                {
                    goingDown = false;
                    goingUp = false;
                }
                previousYPosition = currentYPosition;
            }
            else
            {
                goingDown = false;
                goingUp = false;
                onSlope = false;
            }
        }
    }

    private void AdjustMoveSpeed()
    {
        if (isGrounded)
        {
            if (onSlope)
            {
                if (goingUp)
                {

                }

                if (goingDown)
                {

                }
            }


            if (isWalking)
            {
                speedModifier = 2f;
            }

            if (isRunning)
            {
                speedModifier = 5f;
            }

            if (isSprinting)
            {
                speedModifier = 7f;
            }

            if (isGliding)
            {
                speedModifier = 4.6f;
            }
        }
    }
}
