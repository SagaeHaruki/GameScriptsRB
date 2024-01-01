using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    PlayerMovementV2 instance;
    #region Bools
    [SerializeField] private bool playerState;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isFalling;
    #endregion

    private void Awake()
    {
        instance = GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        isMoving = instance.isMoving;
        isWalking = instance.isWalking;
        isRunning = instance.isRunning;
        isSprinting = instance.isSprinting;

        MovingState();
    }

    public void MovingState()
    {
        if (!isFalling)
        {
            if (isMoving)
            {
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

            if (!isMoving)
            {

            }
        }

        if (isFalling)
        {

        }
    }

    public void JumpingState()
    {

    }

}
