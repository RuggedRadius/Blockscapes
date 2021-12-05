using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streaks : MonoBehaviour
{
    // Streak Counter
    [Header("Streak Counter")]
    public int streakCounter;
    public int streakActiveThreshold;
    public bool streakActive;

    // Multiplier value
    [Header("Streak Value")]
    public float streakMultiplier;

    // Colours
    [Header("Colour History")]
    public Color lastMatchedColour;
    public List<Color> colourHistory;

    


    private void Update()
    {
        if (!streakActive)
        {
            if (streakCounter >= streakActiveThreshold)
            {
                // Enable streak
                streakActive = true;
            }
        }
        else
        {
            if (streakCounter < streakActiveThreshold)
            {
                // Disable streak
                streakActive = false;
            }
        }
    }

    public void IncrementCounter()
    {
        streakCounter++;
    }

    public void ResetCounter()
    {
        streakCounter = 0;
    }

    public void UpdateLastMatchedColour(Color last)
    {
        if (last != lastMatchedColour)
        {
            // Reset counter
            ResetCounter();
        }

        // Increment counter
        IncrementCounter();

        // Add colour to history list, insert to 1st index
        colourHistory.Insert(0, last);

        // Update last colour match
        lastMatchedColour = last;
    }
}
