using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    PlayerMovement instance;
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
        instance = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        isMoving = instance.isMoving;
        isWalking = instance.isWalking;
        isRunning = instance.isRunning;
        isSprinting = instance.isSprinting;
    }

    public void JumpingState()
    {

    }

}
