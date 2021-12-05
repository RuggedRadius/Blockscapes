using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    [Header("State")]
    public bool selecting;
    public bool matching;

    [Header("Selections")]
    public GameObject selection1;
    public GameObject selection2;

    [Header("SFX References")]
    public AudioSource audioSelect;
    public AudioClip[] selectClips;
    public AudioSource audioMatch;
    public AudioClip[] matchClips;


    private void Update()
    {
        // Handle selection
        try
        {
            if (selection1 != null && selection2 != null)
            {
                if (isBlocksMatching(selection1, selection2))
                {
                    StartCoroutine(MatchBlocksAndDestroy(selection1, selection2));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Block destroyed while being procesed in update.\n" + ex.Message);
        }
    }

    private IEnumerator MatchBlocksAndDestroy(GameObject block1, GameObject block2)
    {
        matching = true;

        
        // Get block colour for score/colour history
        Color blockColour = block1.GetComponent<TileScript>().blockColour;

        // Update streaks information
        GameController.streaks.UpdateLastMatchedColour(blockColour);

        // Update score information
        GameController.scoreManager.IncrementScore(GameController.scoreManager.values.blockMatch);


        // Play SFX
        SFXController.PlaySound(SFX.BlockMatch);

        // Creat explosions prefabs
        Instantiate(GameController.prefabs.vfxBlockExplosion, block1.transform.position, Quaternion.Euler(Vector3.right * -90f), null);
        Instantiate(GameController.prefabs.vfxBlockExplosion, block2.transform.position, Quaternion.Euler(Vector3.right * -90f), null);

        // Clear selections
        selection1 = null;
        selection2 = null;
        
        // Destroy both blocks
        GameController.blockController.DestroyBlock(block1);
        GameController.blockController.DestroyBlock(block2);

        // Wait
        yield return new WaitForSeconds(0.25f);

        matching = false;
    }


    private bool isBlocksMatching(GameObject block1, GameObject block2)
    {
        Color selection1Colour = block1.GetComponent<MeshRenderer>().material.color;
        Color selection2Colour = block2.GetComponent<MeshRenderer>().material.color;

        if (selection1Colour == selection2Colour)
        {
            //Debug.Log("Cubes match");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Select(GameObject blockToSelect)
    {
        if (blockToSelect == selection2 ||
            matching)
        {
            return;
        }
        else if (selection1 != blockToSelect)
        {
            selecting = true;

            audioSelect.clip = selectClips[UnityEngine.Random.Range(0, selectClips.Length)];
            audioSelect.Play();

            // Destroy outline on selection #2, if applicable
            if (selection2 != null)
            {
                selection2.GetComponent<Outline>().OutlineWidth = 0f;
            }

            string sel1 = string.Empty;
            string sel2 = string.Empty;
            string sel = string.Empty;
            if (selection1 != null && selection2 != null)
            {
                sel1 = selection1.gameObject.name;
                sel2 = selection2.gameObject.name;
                sel = blockToSelect.gameObject.name;
            }

            // Swap selections
            selection2 = selection1;
            selection1 = blockToSelect;

            // Outline new selection #1
            Outline ol = selection1.GetComponent<Outline>();
            ol.OutlineColor = Color.white;
            ol.OutlineWidth = 10f;

            selecting = false;
        }
    }
}
