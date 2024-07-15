using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimation : MonoBehaviour
{

    [SerializeField, Range(0.01f, 2)] private float rotationSmoothness = 0.05f;
    [SerializeField, Range(2, 20)] private float orientationResetMultiplier = 10f;
    [SerializeField, Range(50, 200)] private float rotationSpeed = 100f;
    [SerializeField, Range(0, 90)] private float rotationCap = 35f; 
    [SerializeField] private float emissionStrength = 15f;
    [SerializeField] private float intensityMultiplier = 2f;
    [SerializeField] private float maxIntensity = 40f;
    [SerializeField] private float minIntensity = 5f;

    private Quaternion _currentOrientation;
    private Quaternion _initialOrientation;
    private Quaternion _nextRotation;

    private MeshRenderer _meshRenderer;
    private Material emission;
    private float _currentIntensity;

    private void Start()
    {
        _currentOrientation = transform.rotation;
        _initialOrientation = transform.rotation;
        _nextRotation = transform.rotation;
        _meshRenderer = GetComponent<MeshRenderer>();
        emission = _meshRenderer.materials[2];
        _currentIntensity = emissionStrength;
    }

    public void HandleEngineEmission(Vector2 moveVector)
    {
        if (moveVector.y > 0.5)
        {
            _currentIntensity++;
        }
        if (moveVector.y < 0.5)
        {
            _currentIntensity--;
        }
        emission.SetColor("_EmissionColor", emission.color * _currentIntensity * intensityMultiplier);
        _currentIntensity = Mathf.Clamp(_currentIntensity, minIntensity, maxIntensity);
    }

    public void RotateShip(Vector2 moveVector)
    {
        float rotationSpeedMultiplier = 1;
        if (Mathf.Abs(moveVector.x) > 0.5)
        {
            float rotationCapCalculated = rotationCap + 270f;

            if (_nextRotation.eulerAngles.x <= Mathf.Abs(rotationCapCalculated))
            {
                _nextRotation *= Quaternion.Euler(moveVector.x * rotationSpeed, 0, 0);
            }
        }
        else
        {
            _nextRotation = _initialOrientation;
            rotationSpeedMultiplier = orientationResetMultiplier;
        }
        _nextRotation = Quaternion.Slerp(_currentOrientation, _nextRotation, rotationSmoothness * rotationSpeedMultiplier);
        transform.rotation = _nextRotation; // Actual moving.
        _currentOrientation = _nextRotation;
    }
}