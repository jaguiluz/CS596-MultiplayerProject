using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalInputManager : MonoBehaviour
{
    [Header("Settings")] 
    public GameObject player1;
    public GameObject player2;
    public InputActionAsset actionAsset;

    // Components
    private PlayerInput _p1;
    private PlayerInput _p2;
    private InputAction _p1Join;
    private InputAction _p2Join;
    private PositionManager _positionManager;
    
    void Start()
    {
        // Get the car position manager
        _positionManager = GetComponent<PositionManager>();
        
        // Find the actions for player joining and enable
        _p1Join = actionAsset.FindAction("Join1");
        _p2Join = actionAsset.FindAction("Join2");

        _p1Join.Enable();
        _p2Join.Enable();
    }

    void Update()
    {
        // Instantiate the player's car with the control scheme
        // Add the instantiated car to the position manager's list
        if (!_p1 && _p1Join.IsPressed())
        {
            _p1 = PlayerInput.Instantiate(player1, controlScheme: "WASD", pairWithDevice: Keyboard.current);
            _p1Join.Disable();
            _positionManager.AddCar(_p1.gameObject.GetComponent<TrackProgress>());
        }

        if (!_p2 && _p2Join.IsPressed())
        {
            _p2 = PlayerInput.Instantiate(player2, controlScheme: "IJKL", pairWithDevice: Keyboard.current);
            _p2Join.Disable();
            _positionManager.AddCar(_p2.gameObject.GetComponent<TrackProgress>());
        }
    }
}
