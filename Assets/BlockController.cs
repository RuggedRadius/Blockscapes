using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public int totalCount;
    public Dictionary<Color, int> blockCounts;

    [HideInInspector] public List<GameObject> blocks;
    [HideInInspector] public List<GameObject> loneColouredBlocks;
    private Color[] colours;

    private void Awake()
    {
        blocks = new List<GameObject>();
        loneColouredBlocks = new List<GameObject>();
        blockCounts = new Dictionary<Color, int>();
    }

    private void Start()
    {
        colours = GameController.colours.colours;
    }

    void Update()
    {
        try
        {            
            for (int i = 0; i < colours.Length; i++)
            {
                int currentCount = 0;
                if (blockCounts.TryGetValue(colours[i], out currentCount))
                {
                    if (currentCount == 1)
                    {
                        GameObject block = GetBlockByColour(colours[i]);
                        if (!loneColouredBlocks.Contains(block))
                        {
                            loneColouredBlocks.Add(block);
                        }

                        //DestroyBlock(GetBlockByColour(colours[i]));
                        GameObject lastOfColour = GetBlockByColour(colours[i]);
                        lastOfColour.GetComponent<TileScript>().lastOfColour = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Block destroyed while being procesed in update.\n" + ex.Message);
        }

        // Trigger destruction of all singular-coloured blocks if thats all that remains
        if(blocks.Count == loneColouredBlocks.Count)
        {
            if (!destroyingLonerBlocks)
                StartCoroutine(DestroyLonerBlocks());
            else
                Debug.LogWarning("Already destroying loner blocks!");
        }
    }

    public void DestroyAllBlocks()
    {
        // Destroy all blocks
        for (int i = 0; i < blocks.Count; i++)
        {
            // Get current block
            GameObject curBlock = blocks[i];

            // Destroy current block
            DestroyBlock(curBlock);
        }
    }

    public bool destroyingLonerBlocks;
    private IEnumerator DestroyLonerBlocks()
    {
        destroyingLonerBlocks = true;

        //yield return new WaitForSeconds(1f);

        for (int i = 0; i < loneColouredBlocks.Count; i++)
        {
            try
            {
                // Destroy loner block
                DestroyBlock(loneColouredBlocks[i]);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Destroying loner block failed: " + ex.Message);
            }
            yield return new WaitForSeconds(0.25f);
        }

        destroyingLonerBlocks = false;
        yield return null;
    }

    public void AddBlockToList(GameObject newBlock)
    {
        blocks.Add(newBlock);

        // Determine block's colour
        Color blockColour = newBlock.GetComponent<TileScript>().blockColour;
        
        // Add colour if not in dictionary already
        if (!blockCounts.ContainsKey(blockColour))
        {
            blockCounts.Add(blockColour, 0);
        }

        // Increment count
        blockCounts[blockColour]++;
    }

    public GameObject GetBlockByColour(Color color)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].GetComponent<TileScript>().blockColour == color)
            {
                return blocks[i];
            }
        }

        Debug.LogWarning("No block with that colour found: " + color.ToString());
        return null;
    }
    public int GetColourCount(Color color)
    {
        int counter = 0;

        foreach (GameObject block in blocks)
        {
            if (block.GetComponent<TileScript>().blockColour == color)
            {
                counter++;
            }
        }

        //Debug.Log(counter + " blocks counted of " + color.ToString());
        return counter;
    }
    public void DestroyBlock(GameObject blockToDestroy)
    {
        StartCoroutine(destroyBlockRoutine(blockToDestroy));
    }

    private IEnumerator destroyBlockRoutine(GameObject blockToDestroy)
    {
        // Record counts
        blockCounts[blockToDestroy.GetComponent<TileScript>().blockColour]--;
        blocks.Remove(blockToDestroy);

        // Audio SFX
        SFXController.PlaySound(SFX.BlockMatch);

        // Increment score
        GameController.scoreManager.IncrementScore(GameController.scoreManager.values.singleBlock);

        // VFX
        Instantiate(GameController.prefabs.vfxBlockExplosion, blockToDestroy.transform.position, Quaternion.Euler(Vector3.right * -90f), null);

        // Destroy object
        Destroy(blockToDestroy);

        yield return null;
    }
}
