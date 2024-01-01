using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    PlayerMovementV2 playerMovement;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        ChangeNotOnSlope();
        ChangeOnSlope();
    }

    private void ChangeNotOnSlope()
    {
        if (!playerMovement.onSlope)
        {
            // Walking
            if (playerMovement.isWalking)
            {
                playerMovement.speedModifier = 0.5f;
            }

            // Running
            if (playerMovement.isRunning)
            {
                playerMovement.speedModifier = 1f;
            }

            // Sprinting
            if (playerMovement.isSprinting)
            {
                playerMovement.speedModifier = 1.5f;
            }
        }
    }

    private void ChangeOnSlope()
    {
        if (playerMovement.onSlope)
        {
            // If the player is going up
            if (playerMovement.goingUp)
            {
                // Walking
                if (playerMovement.isWalking)
                {
                    playerMovement.speedModifier = 0.2f;
                }

                // Running
                if (playerMovement.isRunning)
                {
                    playerMovement.speedModifier = 0.7f;
                }

                // Sprinting
                if (playerMovement.isSprinting)
                {
                    playerMovement.speedModifier = 1.2f;
                }
            }   

            // If the player is going down
            if (playerMovement.goingDown)
            {
                // Walking
                if (playerMovement.isWalking)
                {
                    playerMovement.speedModifier = 0.4f;
                }

                // Running
                if (playerMovement.isRunning)
                {
                    playerMovement.speedModifier = 0.9f;
                }

                // Sprinting
                if (playerMovement.isSprinting)
                {
                    playerMovement.speedModifier = 1.4f;
                }
            }
        }
    }
}
