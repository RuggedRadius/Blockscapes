using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourManager : MonoBehaviour
{
    public Color[] colours;

    public Color GetRandomColour()
    {
        return colours[Random.Range(0, colours.Length)];
    }
}
