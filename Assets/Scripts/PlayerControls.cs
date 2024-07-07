using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //1. Declares the Input Action asset (Created and named in Unity).
    InputActionsAsset input = null;
    //5. Declare a vector for controlling movement.

    public delegate void OnDeath();
    public static OnDeath onDeath;

    public Vector2 MoveVector { get; private set; } = Vector2.zero;

    public event Action OnJump = delegate { };

    private void Awake()
    {
        //2. Get reference to the Input Action asset. Use 'new' operator when using the generated c# script.
        input = new InputActionsAsset();
    }

    private void OnEnable()
    {   
        //3. Enable the input asset.
        input.Enable();
        //3. Subscribe to the events (performed, canceled etc.) found deep in the input asset for each function.
        input.Gameplay.Movement.performed += OnMovementPerformed;
        input.Gameplay.Movement.canceled += OnMovementCanceled;
        input.Gameplay.Jump.performed += OnJumpPerformed;
        input.Gameplay.Jump.started += OnJumpStarted;
    }


    private void OnDisable()
    {
        //3. Disable when not used.
        input.Disable();
        //3. Unsubscribe to the events (performed, canceled etc.) for each function.
        input.Gameplay.Movement.performed -= OnMovementPerformed;
        input.Gameplay.Movement.canceled -= OnMovementCanceled;
        input.Gameplay.Jump.performed -= OnJumpPerformed;
        input.Gameplay.Jump.started -= OnJumpStarted;
        onDeath?.Invoke();
        Debug.Log("OnDisable triggered");
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        //4. Use InputAction.CallbackContext to be able to subscribe to the event (delegate system).
        MoveVector = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        MoveVector = Vector2.zero;
    }


    private void OnJumpStarted(InputAction.CallbackContext context)
    {
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        OnJump();
    }
}
