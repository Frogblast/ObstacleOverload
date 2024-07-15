using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private float steeringSpeed = 25f;
    [SerializeField] private float minJumpFrequency = 0.2f;
    [SerializeField] private float maxForwardsDistance = 15f;
    [SerializeField] private float maxBackwardsDistance = 10f;
    [SerializeField] private float forwardSpeed = 0.9f;

    [SerializeField] private int nJumps = 1;
    public bool isJumping = false;
    private bool initialJumpDone = false;
    private float jumpDelay;
    [SerializeField] private float jumpForceAdded = 100f;
    [SerializeField] private float jumpForceInitial = 40f;
    [SerializeField] private float maxJumpTime = 0.2f; // Time in seconds that the jump force can be added
    [SerializeField] private float addedForceTimeStart = 0.05f;


    private float jumpTimer = 0f;
    [SerializeField] private float edgeJumpHelpTime = .5f;
    private float nonGroundedTimer = 0f;
    private int currentJumps;
    private Vector3 originPosition;

    private PlayerPhysics playerPhysics;


    public static event System.Action OnJump;
    

    private void FixedUpdate()
    {
        if (jumpDelay > 0)
        {
            jumpDelay -= Time.deltaTime;
        }
        ReturnToBaseSpeed();
    }

    private void Update()
    {
        if (isJumping)
        {
            Jump();
        }
        edgeJumpHandicap();

        if (initialJumpDone)
        {
            jumpTimer += Time.deltaTime;
        }
    }

    private bool isGroundedWithMargin()
    {
        return nonGroundedTimer < edgeJumpHelpTime;
    }

    private void edgeJumpHandicap()
    {
        if (!playerPhysics.IsGrounded())
        {
            nonGroundedTimer += Time.deltaTime;
        }
        else
        {
            nonGroundedTimer = 0f;
        }
    }

    private void Awake()
    {
        playerPhysics = GetComponent<PlayerPhysics>();
        currentJumps = nJumps;
        originPosition = playerPhysics.transform.position;
    }

    public void Steer(Vector2 moveInput2D)
    {
        Vector3 moveInput = Vector2Dto3D(moveInput2D);
        if (Mathf.Abs(moveInput.y) <= Mathf.Epsilon)
        {
            ReturnToBaseSpeed();
        }
        float maxSteeringRatio = steeringSpeed - Mathf.Abs(playerPhysics.GetVelocity().x);
        float cappedForwardMovement = AdjustForwardMovement(moveInput.z);
        Vector3 adjustedMovement = new Vector3(moveInput.x, moveInput.y, cappedForwardMovement);
        Vector3 movementForce = adjustedMovement * Time.deltaTime * maxSteeringRatio;

        playerPhysics.ApplyMovement(movementForce, ForceMode.Impulse);
    }

    private float AdjustForwardMovement(float z) // Caps the forward/backwards movement at a certain distance from origin.
    {
        float forwardForce = forwardSpeed * z;
        float currentZPosition = transform.position.z;
        float localMaxForwardDistance = originPosition.z + maxForwardsDistance;
        float localMaxBackwardsDistance = originPosition.z - maxBackwardsDistance;

        bool inForwardSlowDownZone = currentZPosition > localMaxForwardDistance / 2;
        bool inBackwardSlowDownZone = currentZPosition < localMaxBackwardsDistance / 2;
        bool atForwardLimit = currentZPosition >= localMaxForwardDistance;
        bool atBackwardLimit = currentZPosition <= localMaxBackwardsDistance;
        bool inputIsForward = z > Mathf.Epsilon;
        bool inputIsBackward = z < Mathf.Epsilon;

        if (inForwardSlowDownZone && inputIsForward) // Adjusts the acceleration depending on distance from origin for smoother movement.
        {
            forwardForce = atForwardLimit ? 0 : forwardForce * Mathf.Clamp(localMaxForwardDistance - currentZPosition, 0, 1);
        }
        if (inBackwardSlowDownZone && inputIsBackward)
        {
            forwardForce = atBackwardLimit ? 0 : forwardForce * Mathf.Clamp(-localMaxBackwardsDistance + currentZPosition, 0, 1);
        }
        return forwardForce;
    }

    private void ReturnToBaseSpeed()
    { // Forces the player back to origin when there is no forward/backwards input.
        float zDistance = originPosition.z - playerPhysics.transform.position.z;
        Vector3 targetPosition = new Vector3(0, 0, zDistance * 0.5f);
        playerPhysics.ApplyMovement(targetPosition, ForceMode.Force);
    }

    // Jumping

    private void Jump()
    {
       // bool isGrounded = playerPhysics.IsGrounded();
        if (isGroundedWithMargin())
        {
            Debug.Log("Timer reset and grounded");
            jumpTimer = 0f;
            currentJumps = nJumps;
            initialJumpDone = false;
        }

        if (!initialJumpDone && isGroundedWithMargin() && currentJumps > 0)
        {
            OnJump?.Invoke();
            currentJumps--;
            initialJumpDone = true;
            playerPhysics.ApplyMovement(Vector3.up * jumpForceInitial * Time.deltaTime, ForceMode.VelocityChange);
        } 
        else if (initialJumpDone && jumpTimer > addedForceTimeStart && jumpTimer < maxJumpTime)
        {
           // Debug.Log("Added force");
            playerPhysics.ApplyMovement(Vector3.up * jumpForceAdded * Time.deltaTime, ForceMode.VelocityChange);
        }

        if (jumpTimer >= maxJumpTime)
        {
            Debug.Log("Jump time up");
        }
    }



    private Vector3 Vector2Dto3D(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
