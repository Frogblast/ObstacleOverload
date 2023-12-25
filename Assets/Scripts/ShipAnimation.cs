using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimation : MonoBehaviour
{
    private PlayerControls _playerControls;

    [SerializeField, Range(0.01f, 2)] private float rotationSmoothness = 0.05f;
    [SerializeField, Range(2, 20)] private float orientationResetMultiplier = 10f;
    [SerializeField, Range(50, 200)] private float rotationSpeed = 100f;
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
        _playerControls = GetComponentInParent<PlayerControls>();
        _meshRenderer = GetComponent<MeshRenderer>();
        emission = _meshRenderer.materials[2];
        _currentIntensity = emissionStrength;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateShip();
        HandleEngineEmission();
    }

    private void HandleEngineEmission()
    {
        if (_playerControls.MoveVector.y > 0.5)
        {
            _currentIntensity++;
        }
        if (_playerControls.MoveVector.y < 0.5)
        {
            _currentIntensity--;
        }
        emission.SetColor("_EmissionColor", emission.color * _currentIntensity * intensityMultiplier);
        _currentIntensity = Mathf.Clamp(_currentIntensity, minIntensity, maxIntensity);

    }

    private void RotateShip()
    {
        float rotationSpeedMultiplier = 1;
        if (Mathf.Abs(_playerControls.MoveVector.x) > 0.5)
        {
            _nextRotation *= Quaternion.Euler(_playerControls.MoveVector.x * rotationSpeed, 0, 0);
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