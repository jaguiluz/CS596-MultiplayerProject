using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class CarInput : NetworkBehaviour
{
    // Components
    private CarController _cc;
    public InputActionAsset actionAsset;

    private InputAction m_Move;
    void Awake()
    {
        // Get a reference of the CarController and move action
        _cc = GetComponent<CarController>();
        m_Move = actionAsset.FindAction("Move");
    }

    void FixedUpdate()
    {
        if (!IsOwner) return; // Do not have server read inputs
        
        // Read the player input value and set it in the CarController
        Vector2 input = m_Move.ReadValue<Vector2>();
        _cc.SetInputs(input);
    }
}
