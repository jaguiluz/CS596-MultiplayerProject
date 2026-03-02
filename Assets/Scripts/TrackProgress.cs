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

    private List<Checkpoint> _checkpointList;
    
    // Events
    public event Action<TrackProgress> OnPassCheckpoint;

    void Awake()
    {
        // Get a list of all the checkpoints in the track
        _checkpointList = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();
        _checkpointList = _checkpointList.OrderBy(x => x.checkpointNum).ToList();
    }
    
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

    public float GetDistFromNextCheckpoint()
    {
        float dist = Vector3.Distance(_checkpointList[i_CurrentCheckpoint + 1].transform.position, transform.position);
        return dist;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            var checkpoint = other.gameObject.GetComponent<Checkpoint>();
            int checkpointNum = checkpoint.checkpointNum;

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
            var finish = other.gameObject.GetComponent<Checkpoint>();
            int checkpointNum = finish.checkpointNum;

            if (checkpointNum == i_CurrentCheckpoint + 1)
            {
                if (i_CurrentLap + 1 > i_totalLaps) isFinished = true;
                
                i_CurrentCheckpoint = -1;
                i_CurrentLap++;
                f_Time = Time.time;
                OnPassCheckpoint?.Invoke(this);
            }
        }
    }
}
