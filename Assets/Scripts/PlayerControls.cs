using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //1. Declares the Input Action asset (Created and named in Unity).
    InputActionsAsset input;
    //5. Declare a vector for controlling movement.
    public Vector2 MoveVector { get; private set; } = Vector2.zero;
    public bool JumpButtonPressed { get; private set; } = false;

    private PlayerPhysics playerPhysics;

    private void Start()
    {
        //2. Make Input Action asset.
        input = new InputActionsAsset();
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        //4. Use InputAction.CallbackContext to be able to subscribe to the event (delegate system).
        MoveVector = context.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerPhysics.ResetJumpTimer();
            playerPhysics.isJumping = true;
        }
        else if(context.canceled || context.performed)
        {
            playerPhysics.ResetJumpTimer();
            playerPhysics.isJumping = false;
        }
    }
}
