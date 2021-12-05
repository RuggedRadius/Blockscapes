using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer")]
    public float currentRemainingTimer;
    public float currentLevelTime;

    [Header("Timer Settings")]
    public float baseTime;

    [Header("State")]
    public bool timerActive;


    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        // Decrement timer
        if (timerActive)
        {
            currentRemainingTimer -= Time.deltaTime;
        }

        // Check for in-active toggle
        if (timerActive)
        {
            if (currentRemainingTimer <= 0.0f)
            {
                timerActive = false;
            }
        }

        // Update UI text for timer
        GameController.uiController.UpdateUITimer(currentRemainingTimer);
    }

    public void ResetTimer()
    {
        this.currentRemainingTimer = this.currentLevelTime;
    }

    public void AddTimeToTimer(float _time)
    {
        if (_time >= 0f)
        {
            this.currentRemainingTimer += _time;
        }
        else
        {
            Debug.LogWarning("Can't add negative time!");
        }
    }

    public void RemoveTimeFromTimer(float _time)
    {
        if (_time >= 0f)
        {
            this.currentRemainingTimer -= _time;
        }
        else
        {
            Debug.LogWarning("Can't remove negative time!");
        }
    }

    public float calculateLevelTime(int _level)
    {
        // Get block count
        float blockCount = GameController.blockController.blocks.Count;

        // Calculate time
        float time = (Mathf.Sqrt(blockCount) + blockCount + (blockCount/1));

        // Modify time according to game diffculty
        time /= GameController.GCInstance.gameDifficulty;

        // Increase time for landscape
        //time *= 2f;

        // Initialise variable
        return baseTime + time;
    }

    private int countTotalBlocksInPyramid(int x)
    {
        int total = 0;

        for (int i = 0; i < x; i++)
        {
            total += (4 * (i * i)) - (4 * i) + 1;
        }

        return total - 1;
    }
}
