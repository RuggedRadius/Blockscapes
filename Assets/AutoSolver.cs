using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSolver : MonoBehaviour
{
    public static bool solverEnabled;
    private Selector select;



    void Start()
    {
        select = GameController.selector;
        
        if (solverEnabled)
        {
            StartCoroutine(Solver());
        }
    }

    private IEnumerator Solver()
    {
        yield return new WaitForSeconds(1);

        while (solverEnabled)
        {
            foreach (Color c in GameController.colours.colours)
            {
                while (true)
                {
                    // Initialise block selections
                    GameObject block1 = null;
                    GameObject block2 = null;

                    // Select block #1
                    try
                    {
                        // Find block 1
                        block1 = GetBlockByColour(c);

                        // Select block 1
                        select.Select(block1);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("Couldn't select block 1");
                    }

                    // Small wait time between selections
                    //yield return new WaitForSeconds(0.05f);
                    yield return null;

                    // Select block #2
                    try
                    {
                        // Find block 2
                        block2 = GetBlockByColour(c);

                        // Select block 2
                        select.Select(block2);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("Couldn't select block 2");
                    }

                    // Small wait time after selections
                    //yield return new WaitForSeconds(0.05f);
                    yield return null;

                    // If either block remains null, break iteration of current colour and continue with next colour in array
                    if (block1 == null || block2 == null)
                        break;
                }
            }

            yield return null;
        }
    }

    private GameObject GetBlockByColour(Color colourRequest)
    {
        GameObject[] activeBlocks = GameController.blockController.blocks.ToArray();

        for (int i = 0; i < activeBlocks.Length; i++)
        {
            if (activeBlocks[i].GetComponent<TileScript>().blockColour == colourRequest)
            {
                if (select.selection1 != activeBlocks[i] &&
                    select.selection2 != activeBlocks[i])
                {
                    return activeBlocks[i];
                }
            }
        }

        return null;
    }
}
