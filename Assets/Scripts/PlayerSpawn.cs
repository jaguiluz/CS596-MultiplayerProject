using UnityEngine;
using Unity.Netcode;
using Unity.Networking.Transport;

public class PlayerSpawn : MonoBehaviour
{
    [Header("Player Prefabs")] 
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;
    
    void Start()
    {
        // Listen for the client callback and get the client's ID
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
    }

    public void SpawnPlayer(ulong clientId)
    {
        // Do nothing if on client
        if (!NetworkManager.Singleton.IsServer) return;

        GameObject player = null; // Used to store the instantiated player object
        switch (clientId) // Instantiate the player prefab based on the client ID
        {
            case 1:
                player = Instantiate(Player1Prefab);
                break;
            case 2:
                player = Instantiate(Player2Prefab);
                break;
        }
        
        // Spawn the prefab as a network player object
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
