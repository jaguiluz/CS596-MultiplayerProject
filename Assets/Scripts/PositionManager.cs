using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

public class PositionManager : NetworkBehaviour
{
    [Header("Inscribed")]
    public List<TrackProgress> carList = new List<TrackProgress>();
    
    // Internal Values
    public NetworkVariable<bool> raceFinished = new NetworkVariable<bool>(false);
    
    // Components
    public static PositionManager Instance;

    void Awake()
    {
        // Get a static reference for clients
        Instance = this;
    }
    
    /**
     * Add cars to carList when a player joins
     */
    public void AddCar(TrackProgress tProg)
    {
        if (!IsServer) return; // Make sure clients never add to carList
        
        // Add a player car to the car list using the TrackProgress component
        if (carList.Contains(tProg)) return; // Do not add duplicates
        
        Debug.Log($"Adding car {tProg.name}");
        
        carList.Add(tProg);
        // Listen for cars passing checkpoints
        tProg.OnPassCheckpoint += OnPassCheckpoint;
        tProg.OnFinishTrack += OnFinishTrack;
    }

    void OnPassCheckpoint(TrackProgress tProg)
    {
        Debug.Log("Checkpoint passed");
        if (!IsServer) return; // Do not let clients update carList
        
        // Sort the car position list whenever a car passes a checkpoint
        // Sort by lap, then the current checkpoint, then by the time the car passed a checkpoint
        carList = carList.OrderByDescending(l => l.GetCurrentLap())
            .ThenByDescending(c => c.GetCurrentCheckpoint())
            .ThenBy(t => t.GetTime())
            .ToList();

        // Set the car's position
        for (int i = 0; i < carList.Count; i++)
        {
            carList[i].SetCarPosition(i + 1);
        }
    }

    void OnFinishTrack(TrackProgress tProg)
    {
        if (IsServer && !raceFinished.Value) raceFinished.Value = true; // Do not let clients determine race status
    }
}
