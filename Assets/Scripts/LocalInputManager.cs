using UnityEngine;
using UnityEngine.InputSystem;

public class LocalInputManager : MonoBehaviour
{
    [Header("Settings")] 
    public GameObject player1;
    public GameObject player2;
    public InputActionAsset actionAsset;

    private GameObject o_p1;
    private GameObject o_p2;
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
            o_p1 = Instantiate(player1);
        }

        if (!o_p2 && i_p2Join.IsPressed())
        {
            o_p2 = Instantiate(player2);
        }
    }
}
