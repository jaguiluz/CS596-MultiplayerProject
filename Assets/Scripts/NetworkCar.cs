using Unity.Netcode;
using UnityEngine;

namespace WorldManager
{
    public class NetworkCar : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            Position.OnValueChanged += OnStateChanged;
        }

        public override void OnNetworkDespawn()
        {
            Position.OnValueChanged -= OnStateChanged;
        }

        public void OnStateChanged(Vector3 previous, Vector3 current)
        {
// note: `Position.Value` will be equal to `current` here
            if (Position.Value != previous)
            {
                transform.position = Position.Value;
            }
        }
    }
}