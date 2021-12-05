using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public Vector3 cubeSize;

    public int mapHeight = 10;

    public IEnumerator BuildPyrmaid(int tierCount)
    {
        Debug.Log("Building Pyramid");

        // Create new lists of currently active blocks in scene
        InitialiseNewBlockLists(GameController.blockController);

        tierCount -= 1;
        int currentLevelWidth = tierCount;

        // Reset camera target and position (origin) relative to new pyramid size
        Camera.main.GetComponent<CameraController>().ResetNewCameraOrigin(tierCount);

        // Initialise height
        int initialHeightOffset = 0;

        for (int i = 0; i <= tierCount; i++)
        {
            // Each level of pyramid:
            for (int j = -currentLevelWidth; j <= currentLevelWidth; j++)
            {
                // For every block in current level
                for (int k = -currentLevelWidth; k <= currentLevelWidth; k++)
                {
                    // Determine new cube's position
                    Vector3 localPosition = new Vector3(j, i + initialHeightOffset, k);

                    // Spawn cube
                    StartCoroutine(SpawnBlock(localPosition, false));

                    // Increment cube count
                    GameController.blockController.totalCount++;
                }
            }

            // Decrement width of level as pyramid moves to the next level
            currentLevelWidth--;

            string result = "Built layer " + i;

            yield return result;
        }

        // Toggle building level flag in GameLoop
        LevelController.buildingLevel = false;

        yield return null;
    }

    private IEnumerator SpawnBlock(Vector3 localPosition, bool isBlockscape)
    {
        // Offset
        float fallInHeight = 0f;
        Vector3 gridOffset = Vector3.one * 0.5f; // Align to grid
        Vector3 fallInOffset = Vector3.up * fallInHeight; // Fall-in height

        // Apply offsets
        localPosition += gridOffset;
        localPosition += fallInOffset;

        // Create new block
        GameObject newBlock = Instantiate(GameController.prefabs.prefabBlock, localPosition, Quaternion.identity, this.transform);
        
        // Scale block
        newBlock.transform.localScale = cubeSize * 0.5f;
        
        // Name block according to co-ordinates
        newBlock.gameObject.name = string.Format("Row{2} X_{0} Y_{1}", (int)localPosition.x, (int)localPosition.z, (int)localPosition.y);

        if (isBlockscape)
        {
            newBlock.GetComponent<TileScript>().blockColour = calcHeightBasedColour(localPosition.y);
        }
        else
        {
            // Set random colour to block
            newBlock.GetComponent<TileScript>().blockColour = GameController.colours.GetRandomColour();
        }

        // Add to block manager list
        GameController.blockController.AddBlockToList(newBlock);
        yield return null;
    }

    private Color calcHeightBasedColour(float height)
    {
        Debug.Log("Height of new block: " + height);

        if (height >= 8.5f)
            return Color.white;
        else if (height >= 7f)
            return Color.gray;
        else if (height >= 4f)
            return Color.green;
        else if (height >= 2.5f)
            return Color.yellow;
        else
            return Color.blue;
    }

    private void InitialiseNewBlockLists(BlockController _blockManager)
    {
        _blockManager.blocks = new List<GameObject>();
        _blockManager.loneColouredBlocks = new List<GameObject>();
    }

    public IEnumerator BuildPerlinLandscape(int _width, int _height)
    {
        Debug.Log("Building Landscape");

        // Create new lists of currently active blocks in scene
        InitialiseNewBlockLists(GameController.blockController);

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Calculate height from texture
                float heightFloat = Mathf.PerlinNoise(x/10f, y/10f);
                int height = (int)(heightFloat * mapHeight);

                // Determine new cube's position
                Vector3 localPosition = new Vector3(x, height, y);

                // Spawn cube
                StartCoroutine(SpawnBlock(localPosition, false));

                // Increment cube count
                GameController.blockController.totalCount++;

                //yield return null;
            }
        }

        // Reset camera target and position (origin)
        Vector3 cameraCenterPoint = new Vector3(_width / 2, mapHeight/2, _height / 2);
        Camera.main.GetComponent<CameraController>().SetCameraTarget(cameraCenterPoint);

        // Toggle building level flag in GameLoop
        LevelController.buildingLevel = false;

        yield return null;
    }
}
