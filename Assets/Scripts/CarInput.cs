using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    // Components
    private CarController _cc;
    public InputActionAsset actionAsset;

    private InputAction i_accelerate;
    private InputAction i_turn;
    void Awake()
    {
        _cc = GetComponent<CarController>();
        
        i_accelerate = actionAsset.FindAction("Accelerate");
        i_turn = actionAsset.FindAction("Turn");
    }

    void Update()
    {
        Vector2 input = Vector2.zero;
        
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        
        _cc.SetInputs(input);
    }
    
    
}
