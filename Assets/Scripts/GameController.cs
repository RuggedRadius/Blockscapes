using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameController : MonoBehaviour
{


    [Header("Game Settings")]
    [Range(1, 10)] public int gameDifficulty;


    // General
    [Header("References")]
    [SerializeField] public static GameController GCInstance;
    [SerializeField] public static PrefabsManager prefabs;

    
    // Input
    public static InputManager input;
    public static Selector selector;

    // Level and Blocks
    public static ColourManager colours;
    public static BlockController blockController;
    public static LevelController levelController;

    // SFX
    public static SFXController sfxController;

    // UI
    public static UIManager uiController;

    // Score-related 
    public static ScoreManager scoreManager;
    public static Streaks streaks;

    private void Awake()
    {
        colours = this.GetComponentInChildren<ColourManager>();
        input = this.GetComponentInChildren<InputManager>();
        blockController = this.GetComponentInChildren<BlockController>();
        prefabs = this.GetComponentInChildren<PrefabsManager>();
        uiController = this.GetComponentInChildren<UIManager>();
        selector = this.GetComponentInChildren<Selector>();
        levelController = this.GetComponentInChildren<LevelController>();

        sfxController = this.GetComponentInChildren<SFXController>();

        // Score-related
        scoreManager = this.GetComponentInChildren<ScoreManager>();
        streaks = this.GetComponentInChildren<Streaks>();

        GCInstance = this;
    }
}
