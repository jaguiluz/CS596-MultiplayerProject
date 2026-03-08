using UnityEngine;
using TMPro;

public class LapCounter : MonoBehaviour
{
    // Internal values
    private int _totalLaps;
    private int _playerIndex;
    
    // Components
    private TextMeshProUGUI _lapCounter;
    private TrackAttribute _track;
    private TrackProgress _trackProgress;
    
    void Start()
    {
        // Get the necessary components
        _lapCounter = GetComponent<TextMeshProUGUI>();
        _track = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackAttribute>();
        _totalLaps = _track.lapCount;
    }

    public void UpdateLapCount()
    {
        // Get the car's current lap and display it
        int lapCount = _trackProgress.GetCurrentLap();
        int pos = _trackProgress.GetPosition();

        if (lapCount <= _totalLaps)
            _lapCounter.text = $"P{_playerIndex + 1}: {pos}\n" +
                               $"LAP: {lapCount}/{_totalLaps}";
        else
            _lapCounter.text = $"PLAYER {_playerIndex + 1} FINISHED"; // No need to show lap count
    }

    public void SetPlayerRef(int playerIndex, TrackProgress trackProgress)
    {
        _playerIndex = playerIndex;
        _trackProgress = trackProgress;
    }
}
