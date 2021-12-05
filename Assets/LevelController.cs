using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Stats")]
    public int currentLevel;

    [Header("State")]
    public static bool buildingLevel;
    public static bool levelActive;

    // References
    private static LevelBuilder levelBuilder;
    public LevelTimer levelTimer;
    public Scene loadingScene;

    private void Awake()
    {
        levelBuilder = this.GetComponent<LevelBuilder>();
        levelTimer = this.GetComponent<LevelTimer>();
    }

    private void Start()
    {
        levelActive = false;
    }

    public IEnumerator SpawnLevel(int _levelNumber)
    {
        // Create loading screen
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        Scene activeLoadingScreen = SceneManager.GetSceneAt(1);

        while (!activeLoadingScreen.isLoaded)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(activeLoadingScreen);

        yield return null;

        // Toggle building flag (Flag is toggled off in BuildPyramid Coroutine)
        buildingLevel = true;

        // Update current level
        currentLevel = _levelNumber;

        // Spawn level        
        //IEnumerator builder = levelBuilder.BuildPyrmaid(currentLevel);
        //while (builder.MoveNext())
        //{
        //    yield return null;
        //}

        IEnumerator builder = levelBuilder.BuildPerlinLandscape(_levelNumber*5, _levelNumber*5);
        while (builder.MoveNext())
        {
            yield return null;
        }

        // Destroy loading scene
        Scene gameScene = SceneManager.GetSceneByName("Game");
        SceneManager.SetActiveScene(gameScene);
        SceneManager.UnloadSceneAsync(1);

        yield return null;

        // Toggle level active flag
        levelActive = true;

        // Start timer
        GameController.levelController.levelTimer.timerActive = true;

        // Show UI
        GameController.uiController.ToggleLevelFailed(false);
        GameController.uiController.ToggleLevelComplete(false);
        GameController.uiController.ToggleRemainingBlocksDisplay(true);
        GameController.uiController.ToggleLevelTimer(true);

        // Initialise and start timer
        levelTimer.currentLevelTime = levelTimer.calculateLevelTime(_levelNumber);
        levelTimer.currentRemainingTimer = levelTimer.currentLevelTime;
    }

    public void RestartCurrentLevel()
    {
        // End current level
        // Play SFX
        SFXController.PlaySound(SFX.LevelFail);

        // Start current level
        StartLevel(currentLevel);
    }

    public void StartLevel(int _level)
    {
        levelActive = false;

        // Spawn level
        StartCoroutine(SpawnLevel(_level));
    }

    public void ProceedToNextLevel()
    {
        Debug.Log("Proceeding to next level");

        // Increment current level
        currentLevel++;

        // Spawn level
        StartLevel(currentLevel);
    }

    public void CompleteCurrentLevel()
    {
        Debug.Log("Completing level");

        // Play SFX
        SFXController.PlaySound(SFX.LevelComplete);

        // Destroy all blocks
        GameController.blockController.DestroyAllBlocks();

        // Increment score
        GameController.scoreManager.IncrementScore(GameController.scoreManager.values.levelComplete);
        GameController.scoreManager.AddTimerScoreBonus(levelTimer.currentRemainingTimer);

        if (AutoSolver.solverEnabled)
        {
            // Automatically continue to next level
            ProceedToNextLevel();
        }
        else
        {
            // Toggle off UI elements
            GameController.uiController.ToggleLevelFailed(false);
            GameController.uiController.ToggleRemainingBlocksDisplay(false);
            GameController.uiController.ToggleLevelTimer(false);

            // Toggle on UI elements
            GameController.uiController.ToggleLevelComplete(true);
        }

        // Toggle level active flag
        levelActive = false;
    }

    public static void FailCurrentLevel()
    {
        Debug.Log("Failing level");

        // Play SFX
        SFXController.PlaySound(SFX.LevelFail);

        // Destroy all blocks
        GameController.blockController.DestroyAllBlocks();

        // Toggle off UI elements
        GameController.uiController.ToggleLevelComplete(false);
        GameController.uiController.ToggleRemainingBlocksDisplay(false);
        GameController.uiController.ToggleLevelTimer(false);

        // Toggle on UI elements
        GameController.uiController.ToggleLevelFailed(true);

        // Toggle level active flag
        levelActive = false;
    }
}
