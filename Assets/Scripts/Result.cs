using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    // Components
    private TextMeshProUGUI _resultText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _resultText = GetComponent<TextMeshProUGUI>();
        _resultText.color = new Color(255f, 255f, 255f, 0f); // Make the text invisible
    }

    public void SetResults(int playerIndex, bool finish)
    {
        if (!finish)
        {
            _resultText.text = $"PLAYER {playerIndex + 1} WINS!";
            _resultText.color = new Color(255f, 255f, 255f, 255f); // Show the text
        }
    }
}
