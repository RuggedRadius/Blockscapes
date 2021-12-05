using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiCubeCounter : MonoBehaviour
{
    [SerializeField] private Text counterText;

    void Update()
    {
        // Get block count
        int blockCount = GameController.blockController.blocks.Count;

        // Concatenate UI string
        string uiText = string.Format("{0}", blockCount.ToString());

        // Update UI text
        counterText.text = uiText;
    }
}
