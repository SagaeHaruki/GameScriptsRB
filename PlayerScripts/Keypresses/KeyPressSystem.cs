using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressSystem : MonoBehaviour
{
    PlayerMovementV2 playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        ToggleRunWalk();
        ToggleGlide();
        PressSprint();
    }

    private void ToggleGlide()
    {
        if (!playerMovement.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !playerMovement.isGliding)
            {
                RaycastHit hit;

                Ray ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerMovement.layerMasks))
                {
                    float currentHeight = hit.distance;

                    if (currentHeight >= playerMovement.FallingHeightDiff)
                    {
                        if (playerMovement.isFalling)
                        {
                            playerMovement.isFalling = false;
                            playerMovement.isGliding = true;
                        }

                        if (!playerMovement.isFalling)
                        {
                            playerMovement.isGliding = true;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && playerMovement.isGliding)
            {
                playerMovement.isGliding = false;
            }
        }

        if (playerMovement.isGrounded && playerMovement.isGliding)
        {
            playerMovement.isGliding = false;
        }
    }

    private void PressSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !playerMovement.isJumping)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false; 
        }
    }

    private void ToggleRunWalk()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !playerMovement.isJumping)
        {
            if (playerMovement.isRunning)
            {
                playerMovement.isWalking = true;
                playerMovement.isRunning = false;
            }
            else
            {
                playerMovement.isWalking = false;
                playerMovement.isRunning = true;
            }
        }
    }
}
