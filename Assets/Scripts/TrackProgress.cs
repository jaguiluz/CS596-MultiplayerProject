using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrackProgress : NetworkBehaviour
{
    // Network Variables
    public NetworkVariable<int> i_CurrentLap = new NetworkVariable<int>(1);
    public NetworkVariable<int> i_CurrentCheckpoint = new NetworkVariable<int>(-1);
    public NetworkVariable<int> i_CarPosition = new NetworkVariable<int>(0);
    
    // Internal Values
    //private int i_CurrentLap = 1;
    private int i_totalLaps;
    //private int i_CurrentCheckpoint = -1;
    private int i_CheckpointCount = 0;
    //private int i_CarPosition;
    private int i_playerIndex;
    private float f_Time;
    private bool isFinished;
    
    // Components
    private LapCounter _lapCounter;
    private PositionManager _pm;
    private TrackAttribute _track;
    
    // Events
    public event Action<TrackProgress> OnPassCheckpoint;

    public override void OnNetworkSpawn()
    {
        _track = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackAttribute>();
        i_totalLaps = _track.lapCount;
        i_playerIndex = (int) OwnerClientId;
        if (IsServer)
        {
            _pm = PositionManager.Instance;
            _pm.AddCar(this);
        }
        
        switch (i_playerIndex)
        {
            case 1: 
                _lapCounter = GameObject.FindGameObjectWithTag("P1Lap").GetComponent<LapCounter>();
                break;
            case 2: 
                _lapCounter = GameObject.FindGameObjectWithTag("P2Lap").GetComponent<LapCounter>(); 
                break;
        }
        _lapCounter.SetPlayerRef(i_playerIndex, this);
        _lapCounter.UpdateLapCount();

        i_CurrentLap.OnValueChanged += OnLapChange;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return; // Ensure that only the server handles the checkpoint trigger logic
        
        // Check if the player passed a checkpoint or the finish line
        if (other.CompareTag("Checkpoint"))
        {
            // Get the checkpoint that passed
            var checkpoint = other.gameObject.GetComponent<Checkpoint>();
            var checkpointNum = checkpoint.checkpointNum;

            // Check if the passed checkpoint is the next checkpoint
            // Increment the car's checkpoint count and set the current checkpoint to the passed checkpoint
            // Update the time passed
            if (checkpointNum == i_CurrentCheckpoint.Value + 1)
            {
                i_CheckpointCount++;
                i_CurrentCheckpoint.Value = checkpointNum;
                f_Time = Time.time;
                
                OnPassCheckpoint?.Invoke(this);
            }
        }
        else if (other.CompareTag("FinishLine"))
        {
            // Get the finish line object and its checkpoint number
            var finish = other.gameObject.GetComponent<Checkpoint>();
            int checkpointNum = finish.checkpointNum;

            // Check if the finish line is the next checkpoint
            // Increment the car's checkpoint count and set the current checkpoint to -1
            // Update the time passed and increment the lap counter
            if (checkpointNum == i_CurrentCheckpoint.Value + 1)
            {
                // If the player completes the last lap, they are done with the race
                if (i_CurrentLap.Value + 1 > i_totalLaps) isFinished = true;
                
                i_CheckpointCount++;
                i_CurrentCheckpoint.Value = -1;
                i_CurrentLap.Value++;
                f_Time = Time.time;
                
                OnPassCheckpoint?.Invoke(this);
            }
        }
    }

    void OnLapChange(int prev, int next)
    {
        if (_lapCounter) _lapCounter.UpdateLapCount();
    }
    
    
    public void SetCarPosition(int position)
    {
        if (!IsServer) return; // Do not let clients update the car's position
        
        i_CarPosition.Value = position; 
    }

    public int GetCurrentCheckpoint()
    {
        return i_CurrentCheckpoint.Value;
    }

    public int GetCurrentLap()
    {
        return i_CurrentLap.Value;
    }

    public float GetTime()
    {
        return f_Time;
    }
}
