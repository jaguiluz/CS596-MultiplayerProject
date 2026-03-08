using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : NetworkBehaviour
{
    [Header("Car Settings")] 
    public float maxSpeed = 10f;
    public float accelerationSpeed = 30.0f;
    public float turnSpeed = 3.5f;
    public float driftAmt = 1f;

    // Internal Values
    private float _accelerationInput = 0f;
    private float _turnInput = 0f;
    private float _targetAngle = 0f;
    private Vector3 _refVec = Vector3.zero; // Used for smoothly clamping reverse speed
    
    // Components
    private Rigidbody _rb;
    private GameObject _finishLineGO;

    // void Awake()
    // {
    //     // Get references to object components
    //     _rb = GetComponent<Rigidbody>();
    //     _playerInput = GetComponent<PlayerInput>();
    //     _finishLineGO = GameObject.FindGameObjectWithTag("FinishLine");
    //     if (_playerInput != null)
    //     {
    //         // Set the player's index and starting position
    //         _player = _playerInput.playerIndex;
    //         SetStartingPos();
    //     }
    // }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        
        // Get references to object components
        _rb = GetComponent<Rigidbody>();
        _finishLineGO = GameObject.FindGameObjectWithTag("FinishLine");
        
        // Set the player's starting position
        SetStartingPos();
    }

    void FixedUpdate()
    {
        if (!IsServer) return; // Handle object positioning logic through the server
        
        Accelerate();
        Drift();
        Steer();   
    }

    public void SetInputs(Vector2 input)
    {
        // Get client inputs and send to server
        if (IsOwner)
        {
            SendInputServerRpc(input);
        }
    }
    
    [ServerRpc]
    public void SendInputServerRpc(Vector2 input)
    {
        // Set the turning and acceleration inputs from player input
        _turnInput = input.x;
        _accelerationInput = input.y;
    }
    
    void Accelerate()
    {
        // Create an accelerating force
        Vector3 accelerateVector = transform.forward * (_accelerationInput * accelerationSpeed);
        switch (_accelerationInput)
        {
            case 0f:
                // Slow the car down when the player isn't accelerating or reversing using damping
                _rb.angularDamping = Mathf.SmoothStep(_rb.angularDamping, 3.0f, Time.fixedDeltaTime * 3);
                break;
            case var n when n > 0f:
                // Reset damping parameter
                _rb.angularDamping = 0;
                
                // Push the car forward based on its mass and limit its speed
                _rb.AddForce(accelerateVector, ForceMode.Force);
                _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, maxSpeed);
                break;
            case var n when n < 0f:
                // Reset damping parameter
                _rb.angularDamping = 0;
                
                // Reverse the car at half its max speed at most
                _rb.AddForce(accelerateVector, ForceMode.Force);
                Vector3 clampMag = Vector3.ClampMagnitude(_rb.linearVelocity, (maxSpeed / 2f));
                _rb.linearVelocity = Vector3.SmoothDamp(_rb.linearVelocity, 
                    clampMag, ref _refVec, Time.fixedDeltaTime * 3);
                break;
        }
    }

    void Steer()
    {
        // Dampen the turning speed based on the car's velocity
        // Player should not be able to turn the car when not accelerating/reversing
        float turnDampen = _rb.linearVelocity.magnitude / 8;
        turnDampen = Mathf.Clamp01(turnDampen);
        
        // Steer the car based on user input
        _targetAngle += _turnInput * turnSpeed * turnDampen;
        _rb.MoveRotation(Quaternion.Euler(0f, _targetAngle - 90f, 0f));
    }

    void Drift()
    {
        // Take the forward and orthogonal velocities to have the car drift and slow down
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(_rb.linearVelocity, transform.forward);
        Vector3 rightVelocity = transform.right * Vector3.Dot(_rb.linearVelocity, transform.right);
        _rb.linearVelocity =  forwardVelocity + rightVelocity * driftAmt;
    }
    
    /**
     * Set the player's starting position based on their client ID
     */
    void SetStartingPos()
    {
        Vector3 finishLinePos = _finishLineGO.transform.position; // Get the position of the track's finish line
        switch (OwnerClientId)
        {
            // Spawn the second player closer to the finish line
            // Have both cars face the intended direction at the start
            case 1:
                _rb.position = new Vector3(finishLinePos.x + 6f,
                    finishLinePos.y, finishLinePos.z);
                _rb.MoveRotation(Quaternion.Euler(0f, 90f, 0f));
                break;
            case 2:
                _rb.position = new Vector3(finishLinePos.x + 4f,
                    finishLinePos.y, finishLinePos.z);
                _rb.MoveRotation(Quaternion.Euler(0f, 90f, 0f));
                break;
        }
    }
}
