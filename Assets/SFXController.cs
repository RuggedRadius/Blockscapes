using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    LevelComplete,
    LevelFail,
    BlockSelect,
    BlockMatch,
    BlockDestroy
}

public class SFXController : MonoBehaviour
{
    private static AudioSource source;

    private void Awake()
    {
        source = this.GetComponentInParent<AudioSource>();
    }

    public static void PlaySound(SFX _sfx)
    {
        // Select SFX to play
        switch (_sfx)
        {
            case SFX.LevelComplete:
                source.clip = GameController.prefabs.sfxLevelComplete;
                break;

            case SFX.LevelFail:
                source.clip = GameController.prefabs.sfxBlockSelect;
                break;

            case SFX.BlockSelect:
                source.clip = GameController.prefabs.sfxBlockSelect;
                break;

            case SFX.BlockMatch:
                source.clip = GameController.prefabs.sfxBlockMatch;
                break;

            case SFX.BlockDestroy:
                source.clip = GameController.prefabs.sfxBlockSelect;
                break;

            default: break;
        }

        // Play SFX
        source.Play();
    }
}
