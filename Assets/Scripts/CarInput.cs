using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    // Components
    private CarController _cc;
    public InputActionAsset actionAsset;

    private InputAction m_Move;
    void Awake()
    {
        _cc = GetComponent<CarController>();
        m_Move = actionAsset.FindAction("Move");
    }

    void Update()
    {
        Vector2 input = m_Move.ReadValue<Vector2>();
        _cc.SetInputs(input);
    }
}
