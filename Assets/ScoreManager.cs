using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Player Score")]
    public int totalScore;

    [Header("Score Values")]
    public ScoreValues values;

    private Streaks streaks;

    private void Awake()
    {
        streaks = this.GetComponent<Streaks>();
        values = this.GetComponent<ScoreValues>();
    }

    void Start()
    {
        ResetTotalScore();
    }

    private void Update()
    {
        GameController.uiController.UpdateScoreText(totalScore);
    }

    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    public void IncrementScore(int _scoreValue)
    {
        // Apply multipliers
        ApplyMultipliers(_scoreValue);
               
        if (_scoreValue >= 0)
        {
            // Increment score
            totalScore += _scoreValue;
        }
        else
        {
            Debug.LogWarning("Can not increment score with negative or 0 value!");
        }
    }

    public void DecrementScore(int _decrementation)
    {
        if (_decrementation <= 0)
            totalScore -= _decrementation;
        else
            Debug.LogWarning("Can not decrement score with negativeor 0 value!");
    }

    public int ApplyMultipliers(int _score)
    {
        // Streak muiltiplier
        if (streaks.streakActive)
        {
            // Convert score to float
            float scoreFloatVal = (float)_score;

            // Apply multiplier
            scoreFloatVal *= streaks.streakMultiplier;

            // Convert score to int
            _score = (int)scoreFloatVal;
        }

        // Time period bonus
        // ...


        // Return multiplier adjusted score
        return _score;
    }

    public void AddTimerScoreBonus(float _timeRemaining)
    {
        // Calculate time bonus
        float timeBonus = _timeRemaining * 10;

        // Convert float to int
        int bonus = (int)timeBonus;

        // Increment score
        IncrementScore(bonus);
    }
}
