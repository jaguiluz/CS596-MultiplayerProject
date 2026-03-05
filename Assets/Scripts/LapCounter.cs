using UnityEngine;
using TMPro;

public class LapCounter : MonoBehaviour
{
    // Internal values
    private int _totalLaps;
    
    // Components
    private TextMeshProUGUI _lapCounter;
    private TrackAttribute _track;
    
    void Start()
    {
        _lapCounter = GetComponent<TextMeshProUGUI>();
        _track = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackAttribute>();
        _totalLaps = _track.lapCount;
    }

    public void SetLapCount(int lapCount, int playerIndex)
    {
        if (lapCount <= _totalLaps) 
            _lapCounter.text = playerIndex.ToString("P#:\n") + lapCount.ToString("LAP: #/") + _totalLaps;
        else 
            _lapCounter.text = playerIndex.ToString("PLAYER # FINISHED!");
    }
}
