using UnityEngine;
using UnityEngine.InputSystem;

public class LocalInputManager : MonoBehaviour
{
    [Header("Settings")] 
    public GameObject player1;
    public GameObject player2;
    public InputActionAsset actionAsset;

    private PlayerInput o_p1;
    private PlayerInput o_p2;
    private InputAction i_p1Join;
    private InputAction i_p2Join;
    
    void Start()
    {
        i_p1Join = actionAsset.FindAction("Join1");
        i_p2Join = actionAsset.FindAction("Join2");

        i_p1Join.Enable();
        i_p2Join.Enable();
    }

    void Update()
    {
        if (!o_p1 && i_p1Join.IsPressed())
        {
            o_p1 = PlayerInput.Instantiate(player1, controlScheme: "WASD", pairWithDevice: Keyboard.current);
        }

        if (!o_p2 && i_p2Join.IsPressed())
        {
            o_p2 = PlayerInput.Instantiate(player2, controlScheme: "IJKL", pairWithDevice: Keyboard.current);
        }
    }
}
