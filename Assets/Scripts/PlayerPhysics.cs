using System;
using UnityEngine;


public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float groundDetectionRange = 1f;
    [SerializeField] private float hoverHeight = 8.5f;

    public delegate void OnDeath();
    public static OnDeath onDeath;

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
    public bool IsGrounded()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange);
        return Physics.Raycast(ray, groundDetectionRange, ~layerMask);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        onDeath();
    }

    internal void ApplyMovement(Vector3 movementForce, ForceMode forceMode)
    {
        rb.AddForce(movementForce, forceMode);
    }
    internal void ApplyMovement(Vector3 movementForce)
    {
        rb.AddForce(movementForce);
    }

    private void FixedUpdate()
    {
        Hover();
    }

    private void Hover()
    {
        float mass = rb.mass;
        float currentHeight = CalculateRayDistance(Vector3.down);

        // CalculateRayDistance yields 0 if nothing is hit and so the player would fly up forever. Therefore this ugly bugfix.
        currentHeight = (currentHeight == 0) ? groundDetectionRange + 1 : currentHeight;

        if (currentHeight < groundDetectionRange && rb.velocity.y < 0)
        {
            float ratio = 9.81f + mass * hoverHeight;
            rb.AddForce(ratio * Vector3.up * mass);
        }
    }

    private float CalculateRayDistance(Vector3 direction)
    {
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(rb.position, direction, out hitInfo);
        return hitInfo.distance;
    }
}