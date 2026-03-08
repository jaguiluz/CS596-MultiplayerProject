using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;

public class NetworkUI : MonoBehaviour
{
    [Header("UI Buttons")] 
    public TMP_InputField IPAddressField;
    public TMP_InputField PortField;
    public Button hostButton;
    public Button serverButton;
    public Button clientButton;

    private UnityTransport transport;
    
    void Start()
    {
        // Get the UnityTransport component of the NetworkManager
        if (NetworkManager.Singleton) transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        // Listen for each button
        hostButton.onClick.AddListener(StartHost);
        serverButton.onClick.AddListener(StartServer);
        clientButton.onClick.AddListener(StartClient);
    }

    void StartHost()
    {
        transport.SetConnectionData("0.0.0.0", (ushort)7777);
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    void StartServer()
    {
        transport.SetConnectionData("0.0.0.0", (ushort)7777);
        NetworkManager.Singleton.StartServer();
        gameObject.SetActive(false);
    }

    void StartClient()
    {
        string ip = IPAddressField.text;
        ushort port = 7777;
        if (ushort.TryParse(PortField.text, out ushort p)) port = p;
        transport.SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }
}
