using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSystem : MonoBehaviour
{
    PlayerMovementV2 playerMovement;

    float walkingForce = 2.6f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        if (playerMovement.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !playerMovement.isJumping)
            {
                // Apply the upward jump force
                print("a");
                playerMovement.playerRigid.AddForce(Vector3.up * playerMovement.jumpForce, ForceMode.Impulse);
            }
        }
    }
}
