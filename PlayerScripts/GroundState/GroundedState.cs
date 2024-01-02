using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : MonoBehaviour
{
    PlayerMovementV2 playerMovement;

    private float maxRayDistance = 1.0f;
    private float slopeAngle;
    private float previousYPosition;

    private float Gravity = -9.81f;
    private Vector3 Velocity;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        GoingDownSlope();
        UpdateGroundState();
    }

    private void UpdateGroundState()
    {
        if (playerMovement.charControl.isGrounded)
        {
            playerMovement.isGrounded = true;
        }
        
        if(!playerMovement.charControl && !playerMovement.onSlope && !playerMovement.isJumping)
        {
            playerMovement.isGrounded = false;
        }
        
    }

    private void GoingDownSlope()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, maxRayDistance, playerMovement.layerMasks))
        {
            Vector3 groundNormal = hit.normal;
            slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
            float currentYPosition = transform.position.y;
            if (slopeAngle >= 1)
            {
                playerMovement.onSlope = true;
                if (playerMovement.isMoving)
                {
                    if (currentYPosition > previousYPosition)
                    {
                        // Going Up
                        playerMovement.goingDown = false;
                        playerMovement.goingUp = true;
                    }
                    else if (currentYPosition < previousYPosition)
                    {
                        // Going Down
                        playerMovement.goingDown = true;
                        playerMovement.goingUp = false;

                        Velocity.y -= Gravity * -2f * Time.deltaTime;
                        playerMovement.charControl.Move(Velocity * Time.deltaTime);
                    }
                    else
                    {
                        // Neither
                        playerMovement.goingDown = false;
                        playerMovement.goingUp = false;
                        return;
                    }
                }
                else
                {
                    // If not on Moving
                    playerMovement.goingDown = false;
                    playerMovement.goingUp = false;
                }
                previousYPosition = currentYPosition;
            }
            else
            {
                // If not on Slope
                playerMovement.goingDown = false;
                playerMovement.goingUp = false;
                playerMovement.onSlope = false;
            }
        }
    }
}
