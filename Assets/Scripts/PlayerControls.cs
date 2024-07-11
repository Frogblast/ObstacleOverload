using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //1. Declares the Input Action asset (Created and named in Unity).
    InputActionsAsset input;
    //5. Declare a vector for controlling movement.
    public Vector2 MoveVector { get; private set; } = Vector2.zero;
    private float JumpButtonDuration { get; set; } = 0f;
        
    [SerializeField] private float maxPressLength = 1f;

    public Action<float> OnJump;

    private void Start()
    {
        //2. Make Input Action asset.
        input = new InputActionsAsset();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        //4. Use InputAction.CallbackContext to be able to subscribe to the event (delegate system).
        MoveVector = context.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext context)
    {
        float ratio = Mathf.Min((float)context.duration, maxPressLength);
        Debug.Log("Duration: " + context.duration);
        Debug.Log("Ratio: " + ratio);
        OnJump(ratio);
    }

}
