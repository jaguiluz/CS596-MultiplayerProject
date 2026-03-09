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
        // Set the connection data to 0.0.0.0 to allow the host to listen to any incoming signal on port 7777
        transport.SetConnectionData("0.0.0.0", (ushort)7777);
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    void StartServer()
    {
        // Set the connection data to 0.0.0.0 to allow the server to listen to any incoming signal on port 7777'
        transport.SetConnectionData("0.0.0.0", (ushort)7777);
        NetworkManager.Singleton.StartServer();
        gameObject.SetActive(false);
    }

    void StartClient()
    {
        // Get the IP address and port input
        string ip = "127.0.0.1"; // Default if IP address field is empty
        if (!string.IsNullOrWhiteSpace(IPAddressField.text)) ip = IPAddressField.text;
        
        ushort port = 7777; // Default if port field is empty
        if (ushort.TryParse(PortField.text, out ushort p)) port = p;
        
        // Set the connection data based on the input
        transport.SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }
}
