using System;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrackProgress : MonoBehaviour
{
    // Internal Values
    private int i_CurrentLap = 1;
    private int i_totalLaps = 5;
    private int i_CurrentCheckpoint = -1;
    private int i_CheckpointCount = 0;
    private int i_CarPosition;
    private float f_Time;
    private bool isFinished;
    
    // Events
    public event Action<TrackProgress> OnPassCheckpoint;
    
    public void SetCarPosition(int position)
    {
        i_CarPosition = position; 
    }

    public int GetCurrentCheckpoint()
    {
        return i_CurrentCheckpoint;
    }

    public int GetCurrentLap()
    {
        return i_CurrentLap;
    }

    public float GetTime()
    {
        return f_Time;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player passed a checkpoint or the finish line
        if (other.CompareTag("Checkpoint"))
        {
            // Get the checkpoint that passed
            var checkpoint = other.gameObject.GetComponent<Checkpoint>();
            int checkpointNum = checkpoint.checkpointNum;

            // Check if the passed checkpoint is the next checkpoint
            // Increment the car's checkpoint count and set the current checkpoint to the passed checkpoint
            // Update the time passed
            if (checkpointNum == i_CurrentCheckpoint + 1)
            {
                i_CheckpointCount++;
                i_CurrentCheckpoint = checkpointNum;
                f_Time = Time.time;
            }

            OnPassCheckpoint?.Invoke(this);
        }
        else if (other.CompareTag("FinishLine"))
        {
            // Get the finish line object and its checkpoint number
            var finish = other.gameObject.GetComponent<Checkpoint>();
            int checkpointNum = finish.checkpointNum;

            // Check if the finish line is the next checkpoint
            // Increment the car's checkpoint count and set the current checkpoint to -1
            // Update the time passed and increment the lap counter
            if (checkpointNum == i_CurrentCheckpoint + 1)
            {
                // If the player completes the last lap, they are done with the race
                if (i_CurrentLap + 1 > i_totalLaps) isFinished = true;
                
                i_CheckpointCount++;
                i_CurrentCheckpoint = -1;
                i_CurrentLap++;
                f_Time = Time.time;
            }
            
            OnPassCheckpoint?.Invoke(this);
        }
    }
}
