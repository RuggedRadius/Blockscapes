using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject prefabBlock;

    [Header("VFX")]
    public GameObject vfxBlockExplosion;

    [Header("SFX")]
    public AudioClip sfxBlockSelect;
    public AudioClip sfxBlockMatch;
    public AudioClip sfxLevelComplete;

    [Header("UI")]
    public GameObject prefabUILevelComplete;
}
