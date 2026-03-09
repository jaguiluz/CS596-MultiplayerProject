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
    public NetworkVariable<int> i_CarPosition = new NetworkVariable<int>(1);
    public NetworkVariable<bool> isFinished = new NetworkVariable<bool>(false);
    
    // Internal Values
    private int i_totalLaps;
    private int i_playerIndex;
    private float f_Time;
    
    // Components
    private LapCounter _lapCounter;
    private PositionManager _pm;
    private TrackAttribute _track;
    private Result _resultText;
    
    // Events
    public event Action<TrackProgress> OnPassCheckpoint;
    public event Action<TrackProgress> OnFinishTrack;

    public override void OnNetworkSpawn()
    {
        // Get necessary components 
        _track = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackAttribute>();
        _resultText = GameObject.FindGameObjectWithTag("ResultUI").GetComponent<Result>();
        i_totalLaps = _track.lapCount;
        i_playerIndex = (int) OwnerClientId;
        _pm = PositionManager.Instance;
        if (IsServer)
        {
            // Only have the server interact with the carList
            _pm.AddCar(this);
        }
        
        // Based on player index, assign each player to the corresponding lap counter
        switch (i_playerIndex)
        {
            case 0: 
                _lapCounter = GameObject.FindGameObjectWithTag("P1Lap").GetComponent<LapCounter>();
                break;
            case 1: 
                _lapCounter = GameObject.FindGameObjectWithTag("P2Lap").GetComponent<LapCounter>(); 
                break;
        }
        // Set the lap counter's player reference and display the player's current lap
        _lapCounter.SetPlayerRef(i_playerIndex, this);
        _lapCounter.UpdateLapCount();

        // Listen for lap changes and race completion
        i_CurrentLap.OnValueChanged += OnLapChange;
        isFinished.OnValueChanged += OnPlayerFinish;
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
                if (i_CurrentLap.Value + 1 > i_totalLaps)
                {
                    isFinished.Value = true;
                    OnFinishTrack?.Invoke(this);
                }

                i_CurrentCheckpoint.Value = -1;
                i_CurrentLap.Value++;
                f_Time = Time.time;

                OnPassCheckpoint?.Invoke(this);
            }
        }
    }

    void OnLapChange(int prev, int curr)
    {
        if (_lapCounter) _lapCounter.UpdateLapCount();
    }

    void OnPlayerFinish(bool prev, bool curr)
    {
        if (_resultText) _resultText.SetResults(i_playerIndex, _pm.raceFinished.Value);
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

    public int GetPosition()
    {
        return i_CarPosition.Value;
    }

    public float GetTime()
    {
        return f_Time;
    }
}
