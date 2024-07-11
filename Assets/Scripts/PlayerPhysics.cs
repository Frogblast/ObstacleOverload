using System;
using UnityEngine;


public class PlayerPhysics : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody rb;

    [SerializeField] private float steeringSpeed = 25f;
    [SerializeField] private float forwardSpeed = 0.9f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float tapJumpForce = 5f;
    [SerializeField] private int nJumps = 1;
    [SerializeField] private float hoverHeight = 8.5f;
    [SerializeField] private float maxRotation = 35f;
    private int currentJumps;
    [SerializeField] private float groundDetectionRange = 1f;
    [SerializeField] private float minJumpFrequency = 0.2f;
    [SerializeField] private float maxForwardsDistance = 15f;
    [SerializeField] private float maxBackwardsDistance = 10f;
    private Vector3 originPosition;

    public delegate void OnDeath();
    public static OnDeath onDeath;

    private bool isJumping;

    private float jumpDelay;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody>();
        playerControls.OnJump += Jump;

        isJumping = false;
        currentJumps = nJumps;
        originPosition = transform.position;
    }

    private void OnDestroy()
    {
        onDeath();
    }

    private void Jump(float pressDuration)
    {
        if (IsGrounded())
        {
            currentJumps = nJumps;
            isJumping = false;
        }

        //float jumpMultiplier = pressDuration >= 0.1f ? jumpForce : tapJumpForce;
        float jumpMultiplier = pressDuration*10;
        Debug.Log("jumpMultiplier: " + jumpMultiplier);
        if ((currentJumps >= 1))
        {
            currentJumps--;
//            rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
            rb.AddForce(Vector3.up * jumpMultiplier * jumpForce, ForceMode.Impulse);
            Debug.Log("Added force: " + jumpMultiplier);
            jumpDelay = minJumpFrequency;
        }
    }


    private bool IsGrounded()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange);
        return Physics.Raycast(ray, groundDetectionRange, ~layerMask);
    }

    private void Steer()
    {
        Vector3 moveInput = Vector2Dto3D(playerControls.MoveVector);
        float maxSteeringRatio = steeringSpeed - Mathf.Abs(rb.velocity.x);
        float cappedForwardMovement = AdjustForwardMovement(moveInput.z);
        Vector3 adjustedMovement = new Vector3(moveInput.x, moveInput.y, cappedForwardMovement);
        Vector3 movementForce = adjustedMovement * Time.deltaTime * maxSteeringRatio;

        rb.AddForce(movementForce, ForceMode.Impulse);
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
        float ForwardInput = playerControls.MoveVector.y;
        if (Math.Abs(ForwardInput) <= Mathf.Epsilon)
        {
            float zDistance = originPosition.z - rb.transform.position.z;
            Vector3 targetPosition = new Vector3(0, 0, zDistance * 0.5f);
            rb.AddForce(targetPosition, ForceMode.Force);

        }
    }

    private void FixedUpdate()
    {
        if (jumpDelay > 0)
        {
            jumpDelay -= Time.deltaTime;
        }
        Hover();
        Steer();
        ReturnToBaseSpeed();
    }

    private void Hover()
    {
        float ratio;
        float mass = rb.mass;
        float currentHeight = CalculateRayDistance(Vector3.down);        
        
        // CalculateRayDistance yields 0 if nothing is hit and so the player would fly up forever. Therefore this ugly bugfix.
        currentHeight = (currentHeight == 0) ? groundDetectionRange + 1 : currentHeight;

        if (currentHeight < groundDetectionRange)
        {
            ratio = hoverHeight - currentHeight;
            if (currentHeight < 2 * groundDetectionRange / 3)
            {
                ratio = 9.81f + mass * hoverHeight;
            }
            rb.AddForce(ratio * Vector3.up * mass);
        }
    }

    private float CalculateRayDistance(Vector3 direction)
    {
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(rb.position, direction, out hitInfo);
        return hitInfo.distance;
    }

    private Vector3 Vector2Dto3D(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
