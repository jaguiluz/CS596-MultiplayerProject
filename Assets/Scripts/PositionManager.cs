using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PositionManager : MonoBehaviour
{
    [Header("Inscribed")]
    public List<TrackProgress> carList = new List<TrackProgress>();

    public void AddCar(TrackProgress tProg)
    {
        // Add a player car to the car list using the TrackProgress component
        carList.Add(tProg);
        
        // Listen for cars passing checkpoints
        carList.Last().OnPassCheckpoint += OnPassCheckpoint;
    }

    void OnPassCheckpoint(TrackProgress tProg)
    {
        // Sort the car position list whenever a car passes a checkpoint
        // Sort by lap, then the current checkpoint, then by the time the car passed a checkpoint
        carList = carList.OrderByDescending(l => l.GetCurrentLap())
            .ThenByDescending(c => c.GetCurrentCheckpoint())
            .ThenBy(t => t.GetTime())
            .ToList();

        // Set the car's position
        int carPos = carList.IndexOf(tProg) + 1;
        tProg.SetCarPosition(carPos);
    }
}
