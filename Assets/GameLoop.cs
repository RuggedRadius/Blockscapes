using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [Header("Game State")]
    [HideInInspector] public int currentLevel;
    public bool uiShown;



    void Start()
    {
        // Start game loop
        //StartCoroutine(gameLoop());
        // Spawn level
        GameController.levelController.StartLevel(GameController.levelController.currentLevel);
    }

    private void Update()
    {
        if (LevelController.levelActive)
        {
            // Check for completed level
            if (GameController.blockController.blocks.Count == 0)
            {
                Debug.Log("No blocks left");
                GameController.levelController.CompleteCurrentLevel();
            }

            // Check for failed level
            if (GameController.levelController.levelTimer.currentRemainingTimer <= 0.0f)
            {
                Debug.Log("No time left");
                LevelController.FailCurrentLevel();
            }
        }
    }
}
