using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressCheck : MonoBehaviour
{
    PlayerMovementV2 playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        ToggleRunWalk();
        PressSprint();
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
            print("e");
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
