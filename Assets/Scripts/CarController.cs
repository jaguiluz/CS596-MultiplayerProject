using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float accelerationSpeed = 30.0f;
    public float turnSpeed = 3.5f;

    private float _accelerationInput = 0f;
    private float _turnInput = 0f;
    private float _targetAngle = 0f;
    private int _player = 0;
    
    // Components
    private Rigidbody _rb;
    private PlayerInput _playerInput;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput != null)
        {
            _player = _playerInput.playerIndex;
        }
    }

    void FixedUpdate()
    {
        Accelerate();
        Steer();
    }

    public int GetPlayerNum()
    {
        return _player;
    }

    public void SetInputs(Vector2 input)
    {
        _turnInput = input.x;
        _accelerationInput = input.y;
    }
    
    void Accelerate()
    {
        // Create an accelerating force and push the car forward based on its mass
        Vector3 accelerateVector = transform.forward * _accelerationInput * accelerationSpeed;
        _rb.AddForce(accelerateVector, ForceMode.Force);
    }

    void Steer()
    {
        // Steer the car based on user input
        _targetAngle -= _turnInput * turnSpeed;
        _rb.MoveRotation(Quaternion.Euler(0f, _targetAngle, 0f));
    }
}
