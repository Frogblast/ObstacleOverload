using System;
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
    [SerializeField] private float landingForce = 2f;

    [SerializeField] private float edgeJumpHelpTime = .5f;
    private float nonGroundedTimer = 0f;
    [SerializeField] private int currentJumps;
    private int extraJump = 0;
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
        if (isGroundedWithMargin() && !isJumping)
        {
            if (currentJumps <= 0) currentJumps = nJumps;
        }
        if (isJumping)
        {
            ApplyJumpForce();
        }
        edgeJumpHandicap();
    }


    private void Awake()
    {
        playerPhysics = GetComponent<PlayerPhysics>();
        currentJumps = nJumps;
        originPosition = playerPhysics.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExtraJump"))
        {
            Debug.Log("Entered extra jump");
            extraJump++;
            Destroy(other.gameObject);
        }

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
    public void StartJump()
    {

        // Reset the jump
        if (isGroundedWithMargin())
        {
            initialJumpDone = false;
            isJumping = true;
        }
        // Pressing jump again while in the air
        else if (currentJumps > 0)
        {
            isJumping = true;
        }
        else if (extraJump > 0)
        {
            initialJumpDone = false;
            isJumping = true;
            extraJump--;
        }
        else if (currentJumps == 0)
        {
            Debug.Log("No more jumps available");
        }
        if (currentJumps >= 0)
        {
            currentJumps--; // prevent to ever falling below zero
        }
    }

    public void StopJump()
    {
        isJumping = false;
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

    internal void Land(Vector2 moveVector)
    {
        if (moveVector.y < 0)
        {
            playerPhysics.ApplyMovement(Vector3.down * landingForce);
        }
    }

    private void ApplyJumpForce()
    {
        if (!initialJumpDone)
        {

            //Debug.Log("Initial");
            OnJump?.Invoke(); // For soundeffect
            initialJumpDone = true;
            playerPhysics.ApplyMovement(Vector3.up * jumpForceInitial * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (initialJumpDone)
        {
            //Debug.Log("Follow up");

            playerPhysics.ApplyMovement(Vector3.up * jumpForceAdded * Time.deltaTime, ForceMode.VelocityChange);
        }
    }



    private Vector3 Vector2Dto3D(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
