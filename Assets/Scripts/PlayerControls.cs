using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //1. Declares the Input Action asset (Created and named in Unity).
    InputActionsAsset input;
    private Vector2 moveVector = Vector2.zero;
  //  public bool JumpButtonPressed { get; private set; } = false;

    private MovementHandler movementHandler;
    private ShipAnimation shipAnimation;

    private void Awake()
    {
        //2. Make Input Action asset.
        input = new InputActionsAsset();
        movementHandler = GetComponent<MovementHandler>();
        shipAnimation = GetComponentInChildren<ShipAnimation>();

    }

    private void FixedUpdate()
    {
        movementHandler.Steer(moveVector);
        shipAnimation.HandleEngineEmission(moveVector);
        shipAnimation.RotateShip(moveVector);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movementHandler.ResetJumpTimer();
            movementHandler.isJumping = true;
        }
        else if (context.canceled || context.performed)
        {
            movementHandler.ResetJumpTimer();
            movementHandler.isJumping = false;
        }
    }
}
