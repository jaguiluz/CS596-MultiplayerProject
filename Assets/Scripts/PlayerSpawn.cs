using UnityEngine;
using Unity.Netcode;
using Unity.Networking.Transport;

public class PlayerSpawn : MonoBehaviour
{
    [Header("Player Prefabs")] 
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(ulong clientId)
    {
        // Do nothing if on client
        if (!NetworkManager.Singleton.IsServer) return;

        GameObject player = null;
        switch (clientId)
        {
            case 1:
                player = Instantiate(Player1Prefab);
                break;
            case 2:
                player = Instantiate(Player2Prefab);
                break;
        }
        
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
