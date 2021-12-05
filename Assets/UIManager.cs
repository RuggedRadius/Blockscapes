using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Parent Components")]
    public GameObject levelComplete;
    public GameObject levelFailed;
    public GameObject levelRemainingBlocks;
    public GameObject levelTimer;
    public GameObject levelScorePanel;

    [Header("Updateable Text Components")]
    public Text timerText;
    public Text levelScore;




    public void ToggleLevelComplete(bool _value)
    {
        // Toggle level complete dialogue
        levelComplete.SetActive(_value);
    }

    public void ToggleLevelFailed(bool _value)
    {
        // Toggle level failed dialogue
        levelFailed.SetActive(_value);
    }

    public void ToggleRemainingBlocksDisplay(bool _value)
    {
        // Toggle score display
        levelRemainingBlocks.SetActive(_value);
    }

    public void ToggleLevelTimer(bool _value)
    {
        // Toggle level timer
        levelTimer.SetActive(_value);
    }

    public void UpdateUITimer(float _remainingTime)
    {
        timerText.text = _remainingTime.ToString("0.00");
    }

    public void UpdateScoreText(int _score)
    {
        levelScore.text = _score.ToString();
    }
}
