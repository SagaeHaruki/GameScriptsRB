using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingSystem : MonoBehaviour
{
    PlayerMovementV2 playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        GlidePlayer();
    }

    private void GlidePlayer()
    {
        if (playerMovement.isGliding)
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerMovement.layerMasks))
            {
                float currentHeight = hit.distance;
                if (currentHeight >= playerMovement.GlidingHeightDiff)
                {
                    // Calculate the desired velocity while gliding
                    Vector3 desiredVelocity = transform.forward * playerMovement.glideSpeed;

                    // Limit the maximum speed while gliding
                    if (desiredVelocity.magnitude > playerMovement.maxGlideSpeed)
                    {
                        desiredVelocity = desiredVelocity.normalized * playerMovement.maxGlideSpeed;
                    }

                    // Set the Rigidbody's velocity directly
                    playerMovement.playerRigid.velocity = desiredVelocity;

                    // Apply a different downward force while gliding
                    //playerMovement.playerRigid.AddForce(Vector3.down * playerMovement.glideDownForceWhileGliding, ForceMode.Acceleration);
                  
                }
            }
        }
        else
        {
            playerMovement.playerRigid.useGravity = true;
            playerMovement.playerRigid.drag = 0f;
        }
    }
}
