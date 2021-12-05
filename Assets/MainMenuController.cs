using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject highscoresUI;

    private bool gameIsLoading;

    public void LoadGameScene()
    {
        if (!gameIsLoading)
        {
            gameIsLoading = true;

            // Load game scene
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("Game is already loading");
        }
    }

    public void LoadDemoScene()
    {
        if (!gameIsLoading)
        {
            gameIsLoading = true;

            // Load game scene
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);

            // Turn on auto-solver
            AutoSolver.solverEnabled = true;
        }
        else
        {
            Debug.LogWarning("Game is already loading");
        }
    }

    public void ViewHighscores()
    {
        // Show highscores menu
        highscoresUI.SetActive(true);

        // Hide main menu
        mainMenuUI.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");

        // Exit game
        Application.Quit();
    }

    public void CloseHighscoreScreen()
    {        
        // Show highscores menu
        highscoresUI.SetActive(false);

        // Hide main menu
        mainMenuUI.SetActive(true);
    }
}
