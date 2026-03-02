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
        carList.Add(tProg);
        // carList.Last().OnPassCheckpoint += OnPassCheckpoint;
    }

    void Update()
    {
        carList = carList.OrderByDescending(l => l.GetCurrentLap())
            .ThenByDescending(c => c.GetCurrentCheckpoint())
            .ThenBy(d => d.GetDistFromNextCheckpoint())
            .ToList();

        foreach (TrackProgress car in carList)
        {
            int pos = carList.IndexOf(car) + 1;
            car.SetCarPosition(pos);
        }
    }

    // void OnPassCheckpoint(TrackProgress tProg)
    // {
    //     carList = carList.OrderByDescending(l => l.GetCurrentLap())
    //         .ThenByDescending(c => c.GetCurrentCheckpoint())
    //         .ThenBy(t => t.GetTime())
    //         .ToList();
    //
    //     int carPos = carList.IndexOf(tProg) + 1;
    //     tProg.SetCarPosition(carPos);
    // }
}
